using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ThoughtBubble;

public class AIStateBaseNode:MonoBehaviour
{
    public enum BehaviourState
    {
        PREPARE_STATE = 0,
        SELF_ACTION_STATE = 1,
        END_STATE = 2,
    }
    public virtual bool IsActive() { return isActive; }
    public virtual bool IsValid() { return false; }
    public virtual void StartBehaviour() { }
    public virtual void End(bool sucess = true) { }

    private float progress;
    public bool isActive;
    private BehaviourState currentState;
    private bool isEnd;
    protected IEnumerator walkToTaskPoint;
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
    public BehaviourState CurrentState
    {
        get { return currentState; }
        set { currentState = value; }
    }

    public virtual void UpdateCurrentState(BehaviourState state)
    {
        CurrentState = state;
        if (CurrentState == BehaviourState.PREPARE_STATE) PrintToScreen("Prepare state");
        else if (CurrentState == BehaviourState.SELF_ACTION_STATE) PrintToScreen("Self action state");
        else PrintToScreen("End state");
    }
    public virtual void StartWalking() { this.GetComponentInChildren<Animator>().SetBool("IsWalk", true); }
    public virtual void StopWalking() { this.GetComponentInChildren<Animator>().SetBool("IsWalk", false); }
    public virtual void ShowCurious() { this.GetComponentInChildren<Animator>().SetTrigger("Curious"); }
    public void PrintToScreen(string log) { GameObject.FindGameObjectWithTag("LogSystem").GetComponent<LogSystem>().AddLog(log); }
    public void ShowBubble(BubbleType type) 
    { 
        this.transform.parent.gameObject.GetComponentInChildren<ThoughtBubble>().ShowBubble(type);
        ShowCurious();
    }
    public void HideBubble() { this.transform.parent.gameObject.GetComponentInChildren<ThoughtBubble>().HideBubble(); }
    public IEnumerator MoveToTaskPoint(Transform transform, Vector3 position, float time)
    {
        this.GetComponentInChildren<Animator>().SetFloat("Speed", 0.4f);
        this.StartWalking();
        this.transform.LookAt(position);
        Vector3 currentPos = transform.position;
        float timer = 0f;
        while (timer < 1)
        {
            timer += Time.deltaTime / time;
            transform.position = Vector3.Lerp(currentPos, position, timer);
            yield return null;
        }
        this.StopWalking();
        this.GetComponentInChildren<Animator>().SetFloat("Speed", 1f);
    }
}
