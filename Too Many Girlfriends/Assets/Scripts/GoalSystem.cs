using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GoalType
{
    NONE = 0,
    SHOPPING = 1,
    ICECREAM = 2,
    FLOWER = 3,
    KABAB = 4,
    GRILL = 5,
}

public class GoalSystem : MonoBehaviour
{
    public GoalType FinalGoal;
    public int TasksNumberBeforeGoal;
    public GoalType[] goalList;
    private int currentIndex;

    public string GetGoalTypeString(int index)
    {
       string[] texts = new string[]
       {
           "None",
       //    "Restaurant",
           "Buy Clothes",
           "Buy Icecream",
           "Buy Flower",
           "Kebab",
           "Grill",
       };
        return texts[index];
    }
    

    // Start is called before the first frame update
    void Start()
    {
        generateRandomTaskList();
    }
    private void generateRandomTaskList()
    {
        goalList = new GoalType[TasksNumberBeforeGoal+1];
        // - 1 because NONE
        int length = Enum.GetValues(typeof(GoalType)).Length - 1;
        if (TasksNumberBeforeGoal + 1> length)
        {
            TasksNumberBeforeGoal = length - 1;
        }
        //Init the goal
        goalList[TasksNumberBeforeGoal] = FinalGoal;
        for(int i = 0; i < TasksNumberBeforeGoal; i++)
        {
            goalList[i] = GoalType.NONE;
        }
        for(int i = 0; i < TasksNumberBeforeGoal;i++)
        {
            int random = UnityEngine.Random.Range(1, length + 1);
            while(hasGoalInTheList((GoalType)random))
            {
                random = UnityEngine.Random.Range(1, length + 1);
            }
            goalList[i] = (GoalType)random;
        }
    }
    private bool hasGoalInTheList(GoalType type)
    {
        for(int i = 0; i < TasksNumberBeforeGoal + 1; i++)
        {
            if(goalList[i] == type)
            {
                return true;
            }
        }
        return false;
    }

    public GoalType GetCurrentGoal()
    {
        return goalList[currentIndex];
    }

    public bool HandleGoalFinished(GoalType type)
    {
        if(type == GetCurrentGoal())
        {
            if(currentIndex < TasksNumberBeforeGoal)
            {
                currentIndex++;
            }
            else
            {
                this.GetComponent<AIBehaviour>().IsTaskSuccess = true;
            }
            return true;
        }
        return false;
    }
    public string GetFinalGoalText()
    {
        return GetGoalTypeString((int)FinalGoal);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
