using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogSystem : MonoBehaviour
{
    public GameObject[] LogLines;
    public int MaxLines;
    private string[] logs;
    private int logNumber;
    // Start is called before the first frame update

    private void Awake()
    {
        logs = new string[MaxLines];
        logNumber = 0;
    }
    void Start()
    {
        updateLogLines();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddLog(string log)
    {
        //if log is full
        if(logNumber == MaxLines)
        {
            for(int i = 0; i < MaxLines - 1; i++)
            {
                logs[i] = logs[i + 1];
            }
            logs[MaxLines - 1] = log;
        }
        else
        {
            logs[logNumber] = log;
            logNumber++;
        }
        updateLogLines();
    }

    private void updateLogLines()
    {
        for(int i = 0; i < LogLines.Length; i++)
        {
            LogLines[i].GetComponent<Text>().text = logs[i];
        }
    }

}
