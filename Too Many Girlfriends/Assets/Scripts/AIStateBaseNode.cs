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
    }
    public virtual void StartWalking() { this.GetComponentInChildren<Animator>().SetBool("IsWalk", true); }
    public virtual void StopWalking() { this.GetComponentInChildren<Animator>().SetBool("IsWalk", false); }
    public virtual void ShowCurious() { this.GetComponentInChildren<Animator>().SetTrigger("Curious"); }

    public void ShowExclamation() { this.GetComponent<ThoughtBubble>().ShowExclamation(); }
    public void HideExclamation() { this.GetComponent<ThoughtBubble>().HideExclamation(); }
    public void PrintToScreen(string term) 
    {
        SetCurrentPlayerNameToGM();
        string log = I2.Loc.LocalizationManager.GetTranslation(term);
        I2.Loc.LocalizationManager.ApplyLocalizationParams(ref log);
        GameObject.FindGameObjectWithTag("LogSystem").GetComponent<LogSystem>().AddLog(log); 
    }
    public void ShowBubble() 
    { 
        this.transform.parent.gameObject.GetComponentInChildren<ThoughtBubble>().ShowBubble();
    }
    public void HideBubble() { this.transform.parent.gameObject.GetComponentInChildren<ThoughtBubble>().HideBubble(); }
    public IEnumerator MoveToTaskPoint(Transform transform, Vector3 position, float time)
    {
        ShowCurious();
        ShowExclamation();
        yield return new WaitForSeconds(1.5f);
        HideExclamation();
        this.GetComponent<Rigidbody>().isKinematic = false;
        this.GetComponentInChildren<Animator>().SetFloat("Speed", 0.4f);
        this.StartWalking();
        Vector3 currentPos = transform.position;
        float timer = 0f;
        time -= 1.5f;
        while (timer < 1)
        {
            this.transform.LookAt(position);
            timer += Time.deltaTime / time;
            transform.position = Vector3.Lerp(currentPos, position, timer);
            yield return null;
        }
        this.StopWalking();
        this.GetComponentInChildren<Animator>().SetFloat("Speed", 1f);
       // this.GetComponent<Rigidbody>().isKinematic = true;
    }
    public float GetDistance(GameObject AIPlayer, GameObject MyPlayer)
    {
        Vector3 AIPlayerPos = AIPlayer.transform.position;
        Vector3 MyPlayerPos = MyPlayer.transform.position;
        return Vector3.Distance(AIPlayerPos, MyPlayerPos);
    }
    public int GetShopProgressTime(GameObject shop)
    {
        return shop.GetComponent<ShopBase>().GetProgressTime();
    }

    public void SetCurrentPlayerNameToGM()
    {
        GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>().SetCurrentSpeakerName(this.GetComponent<AIBehaviour>().GetPlayerName());
    }
}
