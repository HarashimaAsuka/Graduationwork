using System.Collections;
using UnityEngine;

[System.Serializable]
public class QuizData
{
    public AudioClip sound;      // 鳴き声
    public string answer;        // "LIGHT1"など
}

public class QuizManager : MonoBehaviour
{
    [Header("問題データ")]
    [SerializeField] private QuizData[] quizzes;

    [Header("鳴き声用AudioSource")]
    [SerializeField] private AudioSource animalAudioSource;

    [Header("効果音・音声用AudioSource")]
    [SerializeField] private AudioSource seAudioSource;

    [Header("正解音")]
    [SerializeField] private AudioClip correctSE;

    [Header("不正解音")]
    [SerializeField] private AudioClip wrongSE;

    [Header("正解数ごとの音声")]
    [SerializeField] private AudioClip[] progressVoices;
    // Element0 = 1問正解
    // Element1 = 2問正解
    // Element2 = 3問正解
    // ...

    [Header("全問正解")]
    [SerializeField] private AudioClip completeVoice;
    [Header("リセット時間（秒）")]
    [SerializeField] private float resetTime = 60f;

    private int currentQuizIndex = 0;
    private string currentAnswer;

    private bool waitingForAnswer = false;

    private int correctCount = 0;
    private int wrongCount = 0;

    // 今回何問連続で正解しているか
    private int streakCount = 0;

    // 最後に手をかざした時間
    private float lastCloseTime;

    void Update()
    {
        // 一分放置でリセット
        if (streakCount > 0 && Time.time - lastCloseTime >= resetTime)
        {
            Debug.Log("1分間操作が無かったのでリセット");

            streakCount = 0;
            correctCount = 0;
            wrongCount = 0;

            currentQuizIndex = 0;
            waitingForAnswer = false;
        }
    }

    public void CheckAnswer(string data)
    {
        if (seAudioSource.isPlaying)
        {
            return;
        }

        //-----------------------------------
        // 手を入れた
        //-----------------------------------
        if (data == "CLOSE")
        {
            lastCloseTime = Time.time;

            if (!waitingForAnswer)
            {
                PlayQuiz();
            }
            else
            {
                ReplayQuiz();
            }

            return;
        }

        //-----------------------------------
        // 問題が出ていない
        //-----------------------------------
        if (!waitingForAnswer)
            return;

        //-----------------------------------
        // 回答
        //-----------------------------------
        if (!data.StartsWith("LIGHT"))
            return;

        //-----------------------------------
        // 正解
        //-----------------------------------
        if (data == currentAnswer)
        {
            Debug.Log("正解");

            correctCount++;
            streakCount++;

            waitingForAnswer = false;

            StartCoroutine(PlayCorrectSequence());

            currentQuizIndex++;

            if (currentQuizIndex >= quizzes.Length)
            {
                currentQuizIndex = 0;
            }
        }
        //-----------------------------------
        // 不正解
        //-----------------------------------
        else
        {
            Debug.Log("不正解");

            wrongCount++;

            if (wrongSE != null)
            {
                seAudioSource.PlayOneShot(wrongSE);
            }

            // waitingForAnswerはtrueのまま
        }

        Debug.Log($"正解:{correctCount} 不正解:{wrongCount}");
    }

    //--------------------------------------------------
    // 問題再生
    //--------------------------------------------------
    void PlayQuiz()
    {
        if (quizzes == null || quizzes.Length == 0)
        {
            Debug.LogWarning("問題がありません");
            return;
        }

        animalAudioSource.Stop();
        animalAudioSource.clip = quizzes[currentQuizIndex].sound;
        animalAudioSource.Play();

        currentAnswer = quizzes[currentQuizIndex].answer;

        waitingForAnswer = true;

        Debug.Log($"問題 {currentQuizIndex + 1}");
        Debug.Log($"正解 {currentAnswer}");
    }

    //--------------------------------------------------
    // 聞き直し
    //--------------------------------------------------
    void ReplayQuiz()
    {
        animalAudioSource.Stop();
        animalAudioSource.time = 0f;
        animalAudioSource.Play();

        Debug.Log("鳴き声を再生");
    }

    //--------------------------------------------------
    // 正解後の音声
    //--------------------------------------------------
        IEnumerator PlayCorrectSequence()
    {
        // ピンポン
        if (correctSE != null)
        {
            seAudioSource.PlayOneShot(correctSE);
            yield return new WaitForSeconds(correctSE.length + 0.2f);
        }

        // 全問正解
        if (streakCount >= quizzes.Length)
        {
            Debug.Log("全問正解！");

            if (completeVoice != null)
            {
                seAudioSource.PlayOneShot(completeVoice);

                // 「全問正解！」の音声が終わるまで待つ
                yield return new WaitForSeconds(completeVoice.length);
            }

            // リセット
            streakCount = 0;
            correctCount = 0;
            wrongCount = 0;

            currentQuizIndex = 0;
            waitingForAnswer = false;

            yield break;
        }

        // ○問正解
        int index = streakCount - 1;

        if (index >= 0 && index < progressVoices.Length)
        {
            AudioClip voice = progressVoices[index];

            if (voice != null)
            {
                seAudioSource.PlayOneShot(voice);
            }
        }
    }
}







        
        // if(streakCount == 8)
        // {
        //     streakCount = 0;
        //     correctCount = 0;
        //     wrongCount = 0;

        //     currentQuizIndex = 0;
        //     waitingForAnswer = false;
        // }