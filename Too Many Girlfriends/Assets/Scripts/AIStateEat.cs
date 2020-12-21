using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIStateEat : AIStateBaseNode
{
    public enum EatType
    {
        NONE,
        RESTAURANT,
        ICECREAM,
    }

    public int CompletionTime = 10;
    public float PrepareStateTime = 5;
    public GameObject ActionBar;
    public GameObject ActionButton;
    private float timerForPrepareState;
    private bool isValid;
    private EatType eatType;
    private bool[] hasEverFinished;
    // Start is called before the first frame update
    void Start()
    {
        isValid = false;
        timerForPrepareState = PrepareStateTime;
        int length = Enum.GetValues(typeof(EatType)).Length;
        hasEverFinished = new bool[length];
        for(int i = 0; i < length; i++)
        {
            hasEverFinished[i] = false;
        }
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
                timerForPrepareState -= Time.deltaTime;
                if(timerForPrepareState <= 0)
                {
                    UpdateCurrentState(BehaviourState.SELF_ACTION_STATE);
                }
           }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Restaurant")
        {
            eatType = EatType.RESTAURANT;
        }
        else if(other.tag == "Icecream")
        {
            eatType = EatType.ICECREAM;
        }
        else
        {
            eatType = EatType.NONE;
        }
        if (((eatType == EatType.ICECREAM && !hasEverFinished[(int)EatType.ICECREAM] ) || (eatType == EatType.RESTAURANT && !hasEverFinished[(int)EatType.RESTAURANT])) && this.GetComponent<AIBehaviour>().GetCurrentBehaviourType()== AIBehaviourType.FOLLOWPLAYER)
        {
            isValid = true;
        }
    }

    void TaskOnClick()
    {
        this.GetComponent<AIStateFollowPlayer>().ForceFollowPlayerState = true;
        this.End(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Restaurant" || other.tag == "Icecream")
        {
            isValid = false;
            eatType = EatType.NONE;
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

        if(eatType == EatType.RESTAURANT)
        {
            ShowBubble(ThoughtBubble.BubbleType.DINNER);
        }
        else if (eatType == EatType.ICECREAM)
        {
            ShowBubble(ThoughtBubble.BubbleType.ICECREAM);
        }
        this.PrintToScreen("EAT STATE START");
    }
    public override void End(bool sucess = true)
    {
        //reset all kinda timer/progression
        Progress = 0;
        timerForPrepareState = PrepareStateTime;
        ActionBar.SetActive(false);
        IsEnd = true;
        isActive = false;
        isValid = false;
   //     hasEverFinished = true;
        ActionButton.SetActive(false);
        if(sucess)
        {
            if(eatType == EatType.RESTAURANT)
            {
                this.GetComponent<GoalSystem>().HandleGoalFinished(GoalType.RESTAURANT);
            }
           else
            {
                this.GetComponent<GoalSystem>().HandleGoalFinished(GoalType.ICECREAM);
            }
            hasEverFinished[(int)eatType] = true;
        }
        eatType = EatType.NONE;
        HideBubble();
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

