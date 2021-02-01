using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
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

    private int CompletionTime;
    private float timerForPrepareState;
    private bool isValid;
    private EatType eatType;
    private bool[] hasEverFinished;
    private Vector3 destPos;
    private Vector3 shopPos;
    private float coolDownTimer;
    // Start is called before the first frame update
    void Start()
    {
        coolDownTimer = 0;
        isValid = false;
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
                this.transform.LookAt(shopPos);
                Progress += Time.deltaTime / CompletionTime;
                this.GetComponent<ThoughtBubble>().UpdateProgressBar(Progress);
                if (Progress >= 1)
                {
                    this.End();
                }
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
           else if(CurrentState == BehaviourState.PREPARE_STATE)
           {
                GameObject mp = GameObject.FindGameObjectWithTag("Player");
                if(mp.GetComponent<PlayerMovement>().HasMoveOutOfShop)
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
        else
        {
            coolDownTimer -= Time.deltaTime;
        }
    }
    public override bool OnPlayerTriggerEnter(Collider other)
    {
        if (!isValid)
        {
            if (other.tag == "Restaurant")
            {
                eatType = EatType.RESTAURANT;
            }
            else if (other.tag == "Icecream")
            {
                eatType = EatType.ICECREAM;
            }
            else if (other.tag == "Kabab")
            {
                eatType = EatType.KEBAB;
            }
            else if (other.tag == "Grill")
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
                if (getGoalTypeFromEatType((EatType)i) == currentGoalType && eatType == (EatType)i && /*!hasEverFinished[i] && */this.GetComponent<AIBehaviour>().GetCurrentBehaviourType() == AIBehaviourType.FOLLOWPLAYER)
                {
                    isValid = true;
                    shopPos = other.gameObject.transform.parent.position;
                    shopPos.y = this.transform.position.y;
                    destPos = other.gameObject.transform.Find("TaskPoint").position;
                    destPos.y = this.transform.position.y;
                    CompletionTime = GetShopProgressTime(other.gameObject.transform.parent.gameObject);
                    timerForPrepareState = GetShopPrepareTime(other.gameObject.transform.parent.gameObject);
                    return true;
                }
            }
        }
        return false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Restaurant" || other.tag == "Icecream" || other.tag == "Kabab" || other.tag == "Grill")
        {
        }
    }

    public override bool IsValid()
    {
        return (isValid && coolDownTimer < 0);
    }
    public override void StartBehaviour()
    {
        Progress = 0;
        IsEnd = false;
        isActive = true;
        UpdateCurrentState(BehaviourState.PREPARE_STATE);

        this.GetComponent<ThoughtBubble>().HandleTaskStart();
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.GetComponent<NavMeshAgent>().enabled = false;
        this.GetComponent<Collider>().enabled = false;

        this.GetComponent<ThoughtBubble>().Bubble.GetComponent<Animator>().SetBool("ShowPulse",true);
    }
    private GoalType getGoalTypeFromEatType(EatType type)
    {
        switch (eatType)
        {
          //  case EatType.RESTAURANT:
          //      return (GoalType.RESTAURANT);
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
        IsEnd = true;
        isActive = false;
        isValid = false;
        if(sucess)
        {
            this.GetComponent<GoalSystem>().HandleGoalFinished(getGoalTypeFromEatType(eatType));
            this.GetComponent<AIStateIdle>().FinishedTask = true;
          //  hasEverFinished[(int)eatType] = true;
        }
        eatType = EatType.NONE;
        UpdateCurrentState(BehaviourState.END_STATE);
        this.GetComponent<Collider>().enabled = true;
        this.GetComponent<NavMeshAgent>().enabled = true;
        this.GetComponent<Rigidbody>().isKinematic = false;
        coolDownTimer = 1;
        this.GetComponent<ThoughtBubble>().HandleTaskEnd();
        this.GetComponent<ThoughtBubble>().Bubble.GetComponent<Animator>().SetBool("ShowPulse", false);
        HideBubble();
    }

    public override void UpdateCurrentState(BehaviourState state)
    {
        if(state == BehaviourState.PREPARE_STATE)
        {
            Progress = 0;
            walkToTaskPoint = MoveToTaskPoint(this.transform, destPos, timerForPrepareState);
            StartCoroutine(walkToTaskPoint);
        }
        else
        {
            this.transform.LookAt(shopPos);
        }
        base.UpdateCurrentState(state);
    }
}

