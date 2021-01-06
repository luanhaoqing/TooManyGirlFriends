using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AIStateShopping : AIStateBaseNode
{
    public enum ShoppingType
    {
        NONE,
        FLOWER,
        CLOTHING,
    }

    public int CompletionTime = 10;
    public float PrepareStateTime = 5;
    public GameObject ActionBar;
    private float timerForPrepareState;
    private bool isValid;
    private ShoppingType shopType;
    private bool[] hasEverFinished;
    private Vector3 destPos;
    private Vector3 shopPos;
    // Start is called before the first frame update
    void Start()
    {
        isValid = false;
        timerForPrepareState = PrepareStateTime;
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
                Progress += Time.deltaTime / CompletionTime;
                ActionBar.GetComponent<Slider>().value = Progress;
                if (Progress >= 1)
                {
                    this.End();
                }
            }
            else if (CurrentState == BehaviourState.PREPARE_STATE)
            {
                GameObject mp = GameObject.FindGameObjectWithTag("Player");
                if (GetDistance(mp, this.gameObject) >= 10)
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
    }

    void OnTriggerEnter(Collider other)
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
            if (getGoalTypeFromShopType((ShoppingType)i) == currentGoalType && shopType == (ShoppingType)i && !hasEverFinished[i] && this.GetComponent<AIBehaviour>().GetCurrentBehaviourType() == AIBehaviourType.FOLLOWPLAYER)
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
        if (other.tag == "Clothing" || other.tag == "Flower" )
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

        switch (shopType)
        {
            case ShoppingType.FLOWER:
                ShowBubble(ThoughtBubble.BubbleType.CLOTHING);
                break;
            case ShoppingType.CLOTHING:
                ShowBubble(ThoughtBubble.BubbleType.CLOTHING);
                break;
          
        }
        this.PrintToScreen("SHOPPING STATE START");
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
        timerForPrepareState = PrepareStateTime;
        ActionBar.SetActive(false);
        IsEnd = true;
        isActive = false;
        isValid = false;
        if (sucess)
        {
            this.GetComponent<GoalSystem>().HandleGoalFinished(getGoalTypeFromShopType(shopType));
            hasEverFinished[(int)shopType] = true;
        }
        shopType = ShoppingType.NONE;
        HideBubble();
        UpdateCurrentState(BehaviourState.END_STATE);
        this.PrintToScreen("SHOPPING STATE END");
    }

    public override void UpdateCurrentState(BehaviourState state)
    {
        if (state == BehaviourState.PREPARE_STATE)
        {
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

