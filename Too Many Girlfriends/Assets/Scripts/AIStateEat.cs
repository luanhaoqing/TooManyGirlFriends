using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIStateEat : AIStateBaseNode
{
    public int CompletionTime = 10;
    public float PrepareStateTime = 5;
    public GameObject ActionBar;
    public GameObject ActionButton;
    private bool isValid;
  //  private bool hasEverFinished;
    // Start is called before the first frame update
    void Start()
    {
        isValid = false;
    //    hasEverFinished = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.IsActive())
        {
            if(CurrentState == BehaviourState.SELF_ACTION_STATE)
            {
                Progress += Time.deltaTime / CompletionTime;
                ActionBar.GetComponent<Slider>().value = Progress;
                if (Progress >= 1)
                {
                    this.End();
                }
            }
           else if(CurrentState == BehaviourState.PREPARE_STATE)
           {
                PrepareStateTime -= Time.deltaTime;
                if(PrepareStateTime<=0)
                {
                    UpdateCurrentState(BehaviourState.SELF_ACTION_STATE);
                }
           }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Restaurant" && this.GetComponent<AIBehaviour>().GetCurrentBehaviourType()== AIBehaviourType.FOLLOWPLAYER)
        {
            isValid = true;
        }
    }

    void TaskOnClick()
    {
        this.GetComponent<AIStateFollowPlayer>().ForceFollowPlayerState = true;
        this.End();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Restaurant")
        {
            isValid = false;
            ActionButton.SetActive(false);
        }
    }

    public override bool IsValid()
    {
        //Need some detection here for entering this node
       // if (hasEverFinished) return false;

        return isValid;
    }
    public override void StartBehaviour()
    {
        Progress = 0;
        ActionBar.SetActive(true);
        IsEnd = false;
        isActive = true;
        UpdateCurrentState(BehaviourState.PREPARE_STATE);
        this.PrintToScreen("EAT STATE START");
    }
    public override void End()
    {
        Progress = 0;
        ActionBar.SetActive(false);
        IsEnd = true;
        isActive = false;
        isValid = false;
   //     hasEverFinished = true;
        ActionButton.SetActive(false);
        this.GetComponent<GoalSystem>().HandleGoalFinished(GoalType.RESTAURANT);
        UpdateCurrentState(BehaviourState.END_STATE);
        this.PrintToScreen("EAT STATE END");
    }

    public override void UpdateCurrentState(BehaviourState state)
    {
        if(state == BehaviourState.PREPARE_STATE)
        {
            ActionButton.SetActive(true);
            Button btn = ActionButton.GetComponent<Button>();
            btn.GetComponentInChildren<Text>().text = "Take Girlfriend Leave";
            btn.onClick.AddListener(TaskOnClick);
        }
        else
        {
            ActionButton.SetActive(false);
        }
        base.UpdateCurrentState(state);
    }
}

