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

public enum FinalGoalType
{
    NONE = 0,
    BUY_SATISFIED_ITEM = 1,
    EAT_ENOUGH = 2,
}

public class GoalSystem : MonoBehaviour
{
    public FinalGoalType FinalGoalType;
    public int TasksNumberBeforeGoalLowerLevel;
    public int TasksNumberBeforeGoalUpperLevel;
    public int MinimumShopNumFinalGoal;
    public GoalType[] goalList;
    public bool DoesLocateASpecificStore;
    private GameObject[] shopObjList;
    private bool[] shopHasArrived;
    private int currentIndex;
    private int totalTasksNum;
    private int preferredShopIndex;
    public string GetGoalTypeString(int index)
    {
       string[] texts = new string[]
       {
           "空",
           "买到心仪的衣服",
           "吃到饱",
       };
        return texts[index];
    }
    

    // Start is called before the first frame update
    void Start()
    {
        DoesLocateASpecificStore = false;
        generateRandomTaskList();
    }
    private void generateRandomTaskList()
    {
        if(FinalGoalType == FinalGoalType.EAT_ENOUGH)
        {
            handleEatEnoughGoalTaskListGeneration();
        }
        else if(FinalGoalType == FinalGoalType.BUY_SATISFIED_ITEM)
        {
            handleBuySatisfiedItemGoalTaskListGeneration();
        }
      
    }
    private void handleEatEnoughGoalTaskListGeneration()
    {
        totalTasksNum = 0;
        int[] tempList = new int[MinimumShopNumFinalGoal];
        for(int i = 0;i< MinimumShopNumFinalGoal;i++)
        {
            tempList[i] = UnityEngine.Random.Range(TasksNumberBeforeGoalLowerLevel, TasksNumberBeforeGoalUpperLevel+1);
            totalTasksNum += tempList[i];
        }
        totalTasksNum += MinimumShopNumFinalGoal;
        goalList = new GoalType[totalTasksNum];

        for(int i = 0;i<totalTasksNum;i++)
        {
            goalList[i] = GoalType.NONE;
        }
        //Init the main goals
        for(int i = 0;i< MinimumShopNumFinalGoal;i++)
        {
            int index = 0;
            for(int j = 0; j <= i; j++)
            {
                index += tempList[j];
            }
            index += i;
            goalList[index] = getRandomEatGoalType();
        }
       
        for (int i = 0; i < totalTasksNum; i++)
        {
            if(goalList[i] == GoalType.NONE)
            {
                goalList[i] = getRandomShoppingGoalType();
            }
        }
    }

    private void handleBuySatisfiedItemGoalTaskListGeneration()
    {
        totalTasksNum = 0;
        int[] tempList = new int[MinimumShopNumFinalGoal];
        shopObjList = GameObject.FindGameObjectsWithTag("Clothing");
        shopHasArrived = new bool[shopObjList.Length];
        for(int i = 0;i<shopObjList.Length;i++)
        {
            shopHasArrived[i] = false;
        }
        for (int i = 0; i < MinimumShopNumFinalGoal; i++)
        {
            tempList[i] = UnityEngine.Random.Range(TasksNumberBeforeGoalLowerLevel, TasksNumberBeforeGoalUpperLevel + 1);
            totalTasksNum += tempList[i];
        }
        totalTasksNum += MinimumShopNumFinalGoal;
        goalList = new GoalType[totalTasksNum];

        for (int i = 0; i < totalTasksNum; i++)
        {
            goalList[i] = GoalType.NONE;
        }
        //Init the main goals
        for (int i = 0; i < MinimumShopNumFinalGoal; i++)
        {
            int index = 0;
            for (int j = 0; j <= i; j++)
            {
                index += tempList[j];
            }
            index += i;
            goalList[index] = GoalType.SHOPPING;
        }

        for (int i = 0; i < totalTasksNum; i++)
        {
            if (goalList[i] == GoalType.NONE)
            {
                goalList[i] = getRandomEatGoalType();
            }
        }
    }
    private void handleBuySatisfiedItemGoalTaskListGenerationFixedStore()
    {
        totalTasksNum = 0;
        currentIndex = 0;
        int randomTaskNum = UnityEngine.Random.Range(TasksNumberBeforeGoalLowerLevel, TasksNumberBeforeGoalUpperLevel + 1);
        totalTasksNum += randomTaskNum;
        totalTasksNum += 1;
   
        goalList = new GoalType[totalTasksNum];

        for (int i = 0; i < totalTasksNum; i++)
        {
            goalList[i] = GoalType.NONE;
        }
        //Init the main goals
        goalList[randomTaskNum] = GoalType.SHOPPING;

        //Here we are trying to find out a specific store in the list which is not visit yet
        int leftShopCount = 0;
        for(int i = 0;i<shopHasArrived.Length;i++)
        {
            if (!shopHasArrived[i]) leftShopCount++;
        }
        int randomShopindex = UnityEngine.Random.Range(0, leftShopCount);
        int tempCount = 0;
        for (int i = 0; i < shopHasArrived.Length; i++)
        {
            if(!shopHasArrived[i])
            {
                if(tempCount == randomShopindex)
                {
                    preferredShopIndex = i;
                }
                else
                {
                    tempCount++;
                }
            }
        }


        for (int i = 0; i < totalTasksNum; i++)
        {
            if (goalList[i] == GoalType.NONE)
            {
                goalList[i] = getRandomEatGoalType();
            }
        }
    }
    private GoalType getRandomEatGoalType()
    {
        GoalType[] typeIncludedForEatType = new GoalType[]
        {
            GoalType.GRILL,
            GoalType.KABAB,
            GoalType.ICECREAM,
        };
        int randomNum = UnityEngine.Random.Range(0, typeIncludedForEatType.Length);
        return typeIncludedForEatType[randomNum];
    }

    private GoalType getRandomShoppingGoalType()
    {
        GoalType[] typeIncludedForShoppingtType = new GoalType[]
        {
            GoalType.SHOPPING,
            GoalType.FLOWER,
        };
        int randomNum = UnityEngine.Random.Range(0, typeIncludedForShoppingtType.Length);
        return typeIncludedForShoppingtType[randomNum];
    }
    private bool hasGoalInTheList(GoalType type)
    {
        for(int i = 0; i < totalTasksNum; i++)
        {
            if(goalList[i] == type)
            {
                return true;
            }
        }
        return false;
    }

    private void handleStoreSeen(GameObject obj)
    {
        for(int i = 0;i< shopObjList.Length;i++)
        {
            if(shopObjList[i] == obj)
            {
                shopHasArrived[i] = true;
            }
        }
    }
    private int getStoreIndex(GameObject obj)
    {
        for (int i = 0; i < shopObjList.Length; i++)
        {
            if (shopObjList[i] == obj)
            {
                return i;
            }
        }
        return -1;
    }
    public GoalType GetCurrentGoal()
    {
        return goalList[currentIndex];
    }

    public bool HandleGoalFinished(GoalType type, GameObject obj = null)
    {
        if(type == GetCurrentGoal())
        {
            if (FinalGoalType == FinalGoalType.BUY_SATISFIED_ITEM && DoesLocateASpecificStore)
            {
                if (currentIndex < totalTasksNum - 1)
                {
                    currentIndex++;
                }
                else
                {
                    if(obj != null && getStoreIndex(obj) == preferredShopIndex)
                    {
                        this.GetComponent<AIBehaviour>().IsTaskSuccess = true;
                    }
                    else
                    {
                        handleBuySatisfiedItemGoalTaskListGenerationFixedStore();
                    }
                }
                return true;
            }
            else
            {
                if (FinalGoalType == FinalGoalType.BUY_SATISFIED_ITEM)
                {
                    if (obj != null)
                    {
                        handleStoreSeen(obj);
                    }
                }

                if (currentIndex < totalTasksNum - 1)
                {
                    currentIndex++;
                }
                else
                {
                    if (FinalGoalType == FinalGoalType.BUY_SATISFIED_ITEM)
                    {
                        DoesLocateASpecificStore = true;
                        handleBuySatisfiedItemGoalTaskListGenerationFixedStore();
                    }
                    else
                    {
                        this.GetComponent<AIBehaviour>().IsTaskSuccess = true;
                    }
                }
                return true;
            }
        }
        
        return false;
    }
    public string GetFinalGoalText()
    {
        return GetGoalTypeString((int)(FinalGoalType));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
