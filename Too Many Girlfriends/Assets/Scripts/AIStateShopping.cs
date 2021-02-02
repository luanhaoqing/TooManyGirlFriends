using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class AIStateShopping : AIStateBaseNode
{
    public enum ShoppingType
    {
        NONE,
        FLOWER,
        CLOTHING,
    }
    private int CompletionTime;
    private float timerForPrepareState;
    private bool isValid;
    private ShoppingType shopType;
    private bool[] hasEverFinished;
    private Vector3 destPos;
    private Vector3 shopPos;
    private float coolDownTimer;
    private GameObject shopObj;
    // Start is called before the first frame update
    void Start()
    {
        coolDownTimer = 0;
        isValid = false;
        int length = Enum.GetValues(typeof(ShoppingType)).Length;
        hasEverFinished = new bool[length];
        for (int i = 0; i < length; i++)
        {
            hasEverFinished[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.IsActive())
        {
            if (CurrentState == BehaviourState.SELF_ACTION_STATE)
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
            else if (CurrentState == BehaviourState.PREPARE_STATE)
            {
                GameObject mp = GameObject.FindGameObjectWithTag("Player");
                if (mp.GetComponent<PlayerMovement>().HasMoveOutOfShop)
                {
                    this.GetComponent<AIStateFollowPlayer>().ForceFollowPlayerState = true;
                    StopCoroutine(walkToTaskPoint);
                    this.End(false);
                }
                timerForPrepareState -= Time.deltaTime;
                if (timerForPrepareState <= 0)
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
            if (other.tag == "Clothing")
            {
                shopType = ShoppingType.CLOTHING;
            }
            else if (other.tag == "Flower")
            {
                shopType = ShoppingType.FLOWER;
            }
            else
            {
                shopType = ShoppingType.NONE;
            }
            int length = Enum.GetValues(typeof(ShoppingType)).Length;
            GoalType currentGoalType = this.GetComponent<GoalSystem>().GetCurrentGoal();
            for (int i = 0; i < length; i++)
            {
                if ((ShoppingType)i == ShoppingType.NONE) continue;
                if (getGoalTypeFromShopType((ShoppingType)i) == currentGoalType && shopType == (ShoppingType)i && /*!hasEverFinished[i] && */this.GetComponent<AIBehaviour>().GetCurrentBehaviourType() == AIBehaviourType.FOLLOWPLAYER)
                {
                    isValid = true;
                    shopPos = other.gameObject.transform.position;
                    shopPos.y = this.transform.position.y;
                    destPos = other.gameObject.transform.Find("TaskPoint").position;
                    destPos.y = this.transform.position.y;
                    CompletionTime = GetShopProgressTime(other.gameObject.transform.parent.gameObject);
                    timerForPrepareState = GetShopPrepareTime(other.gameObject.transform.parent.gameObject);
                    shopObj = other.gameObject;
                    return true;
                }
            }
        }
        return false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Clothing" || other.tag == "Flower" )
        {
        }
    }

    public override bool IsValid()
    {
        return (isValid && coolDownTimer<0);
    }
    public override void StartBehaviour()
    {
        Progress = 0;
        IsEnd = false;
        isActive = true;
        UpdateCurrentState(BehaviourState.PREPARE_STATE);
        
        this.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        this.GetComponent<Collider>().enabled = false;
        this.GetComponent<Rigidbody>().isKinematic = true;
    }
    private GoalType getGoalTypeFromShopType(ShoppingType type)
    {
        switch (shopType)
        {
            case ShoppingType.FLOWER:
                return (GoalType.FLOWER);
            case ShoppingType.CLOTHING:
                return (GoalType.SHOPPING);
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
        if (sucess)
        {
            this.GetComponent<GoalSystem>().HandleGoalFinished(getGoalTypeFromShopType(shopType), shopObj);
            this.GetComponent<AIStateIdle>().FinishedTask = true;
            //    hasEverFinished[(int)shopType] = true;
        }
        shopType = ShoppingType.NONE;

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
        if (state == BehaviourState.PREPARE_STATE)
        {
            Progress = 0;
            walkToTaskPoint = MoveToTaskPoint(this.transform, destPos, timerForPrepareState);
            StartCoroutine(walkToTaskPoint);
        }
        else
        {
            this.transform.LookAt(shopPos);
            this.GetComponent<ThoughtBubble>().HandleTaskStart();
            this.GetComponent<ThoughtBubble>().Bubble.GetComponent<Animator>().SetBool("ShowPulse", true);
        }
        base.UpdateCurrentState(state);
    }
}

