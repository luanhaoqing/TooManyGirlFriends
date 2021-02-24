using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogSystem : MonoBehaviour
{
    public TextMeshProUGUI LogLineDisplay;
    private TextMeshProUGUI[] LogLines;
    public int MaxLines;
    private string[] logs;
    private int logNumber;

    private void Awake()
    {
        logs = new string[MaxLines];
        logNumber = 0;
        LogLines = new TextMeshProUGUI[MaxLines];
        LogLines[0] = LogLineDisplay;
        for (int i = 1; i < MaxLines; ++i)
        {
            LogLines[i] = Instantiate(LogLineDisplay, transform);
        }
    }

    void Start()
    {
        updateLogLines();
    }

    // Update is called once per frame
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
            LogLines[i].text = logs[i];
        }
    }

}
