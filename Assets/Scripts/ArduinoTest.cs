using System.IO.Ports;
using Microsoft.SqlServer.Server;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ArduinoTest : MonoBehaviour
{
    SerialPort serialPort = new SerialPort("COM3", 9600);

    [SerializeField] private string sceneName;

    void Start()
    {
        serialPort.Open();
    }

    void Update()
    {
        if (serialPort.IsOpen && serialPort.BytesToRead > 0)
        {
            string data = serialPort.ReadLine();
            Debug.Log(data);

            if (data.Contains("PUSH"))
        {
            SceneManager.LoadScene(sceneName);
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