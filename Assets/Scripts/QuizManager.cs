using UnityEngine;

[System.Serializable]
public class QuizData
{
    public AudioClip sound; // 鳴き声
    public string answer;   // "LIGHT1" または "LIGHT2"
}

public class QuizManager : MonoBehaviour
{
    [Header("問題データ")]
    [SerializeField] private QuizData[] quizzes;

    [Header("鳴き声用AudioSource")]
    [SerializeField] private AudioSource animalAudioSource;

    [Header("効果音用AudioSource")]
    [SerializeField] private AudioSource seAudioSource;

    [Header("正解音")]
    [SerializeField] private AudioClip correctSE;

    [Header("不正解音")]
    [SerializeField] private AudioClip wrongSE;

    private int currentQuizIndex = 0;
    private string currentAnswer;

    // 回答待ち状態
    private bool waitingForAnswer = false;

    // スコア
    private int correctCount = 0;
    private int wrongCount = 0;

    public void CheckAnswer(string data)
    {
        // 手が近づいたら問題再生
        if (data == "CLOSE")
        {
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

        // 問題が出ていなければ回答不可
        if (!waitingForAnswer)
        {
            return;
        }

        // 回答判定
        if (data.StartsWith("LIGHT"))
        {
            if (data == currentAnswer)
            {
                Debug.Log("正解！");

                correctCount++;

                if (correctSE != null)
                    seAudioSource.PlayOneShot(correctSE);
            }

            Debug.Log($"正解数: {correctCount}  不正解数: {wrongCount}");

            currentQuizIndex++;

            if (currentQuizIndex >= quizzes.Length)
                currentQuizIndex = 0;

            waitingForAnswer = false;
        }

        else
        {
            Debug.Log("不正解！");

            wrongCount++;

            if (wrongSE != null)
                seAudioSource.PlayOneShot(wrongSE);

            Debug.Log($"正解数: {correctCount}  不正解数: {wrongCount}");
        }
    }

    void PlayQuiz()
    {
        if (quizzes == null || quizzes.Length == 0)
        {
            Debug.LogWarning("問題が登録されていません");
            return;
        }

        animalAudioSource.clip = quizzes[currentQuizIndex].sound;
        animalAudioSource.Play();

        currentAnswer = quizzes[currentQuizIndex].answer;

        waitingForAnswer = true;

        Debug.Log($"問題 {currentQuizIndex}");
        Debug.Log($"正解は {currentAnswer}");
    }

    void ReplayQuiz()
    {
        if (animalAudioSource.isPlaying)
            animalAudioSource.Stop();

        animalAudioSource.Play();

        Debug.Log("鳴き声を再生");
    }
}