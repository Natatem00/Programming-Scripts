using UnityEngine;

public class LogManager : MonoBehaviour {

    System.IO.StreamWriter writeToFile;
    string logFile = "log.txt";

    static LogManager logTemp = null;

    void Awake()
    {
        if(logTemp)
        {
            DestroyImmediate(gameObject);
            return;
        }

        logTemp = this;
    }

	void Start () {
        DontDestroyOnLoad(gameObject);

        // creates file
        writeToFile = new System.IO.StreamWriter(Application.persistentDataPath + "/" + logFile);
	}

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTree, LogType type)
    {
        if(type == LogType.Error || type == LogType.Exception)
        {
            Debug.Log(Application.persistentDataPath);
            writeToFile.WriteLine("Logged at: " + System.DateTime.Now.ToString() + " - Log Desc: "
                + logString + " - Trace: " + stackTree + " - Type: " + type.ToString());
        }
    }

    void OnDestroy()
    {
        writeToFile.Close();
    }
	
}
