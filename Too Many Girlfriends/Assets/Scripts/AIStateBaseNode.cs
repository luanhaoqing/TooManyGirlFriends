using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateBaseNode:MonoBehaviour
{
    public virtual bool IsActive() { return isActive; }
    public virtual bool IsValid() { return false; }
    public virtual void StartBehaviour() { }
    public virtual void End() { }

    private float progress;
    public bool isActive;
    private bool isEnd;
    public virtual bool CouldBeOverride() { return false; }

    public float Progress
    {
        get { return progress; }
        set { progress = value; }
    }
    public bool IsEnd
    {
        get { return isEnd; }
        set { isEnd = value; }
    }
    public virtual void StartWalking() { this.GetComponentInChildren<Animator>().SetBool("IsWalk", true); }
    public virtual void StopWalking() { this.GetComponentInChildren<Animator>().SetBool("IsWalk", false); }
    public void PrintToScreen(string log) { GameObject.FindGameObjectWithTag("LogSystem").GetComponent<LogSystem>().AddLog(log); }
}
