using System.IO.Ports;
using UnityEngine;

public class ArduinoManager : MonoBehaviour
{
    SerialPort serialPort = new SerialPort("COM3", 9600);

    [SerializeField] private SceneController sceneController;
    [SerializeField] private bool canPushSceneChange = true;

    public QuizManager quizManager;

    void Start()
    {
        serialPort.Open();
    }

    void Update()
    {
        if (serialPort.IsOpen && serialPort.BytesToRead > 0)
        {
            string data = serialPort.ReadLine().Trim();

            Debug.Log(data);

            // クイズ判定
            if (quizManager != null)
            {
                quizManager.CheckAnswer(data);
            }

            // PUSHでシーン遷移
            if (data == "PUSH")
            {
                Debug.Log("ボタン反応");

                if (canPushSceneChange && sceneController != null)
                {
                    sceneController.ChangeScene();
                }
            }
        }
    }

    void OnDestroy()
    {
        if (serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}