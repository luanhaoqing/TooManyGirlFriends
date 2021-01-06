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
        KEBAB,
        GRILL,
    }

    public int CompletionTime = 10;
    public float PrepareStateTime = 5;
    public GameObject ActionBar;
    private float timerForPrepareState;
    private bool isValid;
    private EatType eatType;
    private bool[] hasEverFinished;
    private Vector3 destPos;
    private Vector3 shopPos;
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
                GameObject mp = GameObject.FindGameObjectWithTag("Player");
                if(GetDistance(mp,this.gameObject) >= 10)
                {
                    this.GetComponent<AIStateFollowPlayer>().ForceFollowPlayerState = true;
                    StopCoroutine(walkToTaskPoint);
                    this.End(false);
                }
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
        else if(other.tag == "Kabab")
        {
            eatType = EatType.KEBAB;
        }
        else if(other.tag == "Grill")
        {
            eatType = EatType.GRILL;
        }
        else
        {
            eatType = EatType.NONE;
        }
        int length = Enum.GetValues(typeof(EatType)).Length;
        GoalType currentGoalType = this.GetComponent<GoalSystem>().GetCurrentGoal();
        for (int i = 0; i < length; i++)
        {
            if ((EatType)i == EatType.NONE) continue;
            if(getGoalTypeFromEatType((EatType)i)== currentGoalType && eatType == (EatType)i && !hasEverFinished[i] && this.GetComponent<AIBehaviour>().GetCurrentBehaviourType() == AIBehaviourType.FOLLOWPLAYER)
            {
                isValid = true;
                shopPos = other.gameObject.transform.position;
                shopPos.y = this.transform.position.y;
                destPos = other.gameObject.transform.Find("TaskPoint").position;
                destPos.y = this.transform.position.y;
                break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Restaurant" || other.tag == "Icecream" || other.tag == "Kabab" || other.tag == "Grill")
        {
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

        switch(eatType)
        {
            case EatType.RESTAURANT:
                ShowBubble(ThoughtBubble.BubbleType.DINNER);
                break;
            case EatType.ICECREAM:
                ShowBubble(ThoughtBubble.BubbleType.ICECREAM);
                break;
            case EatType.KEBAB:
                ShowBubble(ThoughtBubble.BubbleType.KEBAB);
                break;
            case EatType.GRILL:
                ShowBubble(ThoughtBubble.BubbleType.GRILL);
                break;
        }
        this.PrintToScreen("EAT STATE START");
    }
    private GoalType getGoalTypeFromEatType(EatType type)
    {
        switch (eatType)
        {
            case EatType.RESTAURANT:
                return (GoalType.RESTAURANT);
            case EatType.ICECREAM:
                return (GoalType.ICECREAM);
            case EatType.KEBAB:
                return (GoalType.KABAB);
            case EatType.GRILL:
                return (GoalType.GRILL);
        }
        return GoalType.NONE;
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
        if(sucess)
        {
            this.GetComponent<GoalSystem>().HandleGoalFinished(getGoalTypeFromEatType(eatType));
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
            timerForPrepareState = PrepareStateTime;
            Progress = 0;
            walkToTaskPoint = MoveToTaskPoint(this.transform, destPos, PrepareStateTime);
            StartCoroutine(walkToTaskPoint);
        }
        else
        {
            this.transform.LookAt(shopPos);
        }
        base.UpdateCurrentState(state);
    }
}

