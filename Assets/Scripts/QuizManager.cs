using UnityEngine;

public class QuizManager : MonoBehaviour
{
    private string[] answers =
    {
        "LIGHT1",
        "LIGHT2",
        "LIGHT1"
    };

    private int currentQuestion = 0;

    [SerializeField] private SceneController sceneController;

    public void CheckAnswer(string data)
    {
        // 全問題終了済みなら何もしない
        if (currentQuestion >= answers.Length)
        {
            return;
        }

        if (data == "LIGHT1" || data == "LIGHT2")
        {
            // 正解
            if (data == answers[currentQuestion])
            {
                Debug.Log((currentQuestion + 1) + "問目 正解！");

                currentQuestion++;

                // 全問終了チェック
                if (currentQuestion >= answers.Length)
                {
                    Debug.Log("全問題終了！");

                    sceneController.ChangeScene();
                }
            }
            // 不正解
            else
            {
                Debug.Log((currentQuestion + 1) + "問目 外れ！");
            }
        }
    }
}