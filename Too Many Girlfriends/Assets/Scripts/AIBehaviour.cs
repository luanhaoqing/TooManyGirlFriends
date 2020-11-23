using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AIBehaviourType
{
    IDLE = 0,
    WALK = 1,
    EAT = 2,
    FOLLOWPLAYER = 3,
}

public class AIBehaviour : MonoBehaviour
{
    public AIBehaviourType[] BehaviourTypeIncluded;
    public Slider AngryLevelBar;
    private float AngryLevel;
    private int numberOfBehaviour;
    private AIStateBaseNode[] Behaviours;
    private int currentState;
    private float yValue;
    // Start is called before the first frame update
    void Start()
    {
        AngryLevel = 0;
        yValue = this.transform.position.y;
        numberOfBehaviour = BehaviourTypeIncluded.Length;
        Behaviours = new AIStateBaseNode[numberOfBehaviour];

        for (int i =0; i<numberOfBehaviour; i++)
        {
            Behaviours[i] = GetBehaviourByEnum(BehaviourTypeIncluded[i]);
        }
        for (int i = 0; i < numberOfBehaviour; i++)
        {
            if (Behaviours[i].IsValid())
            {
                currentState = i;
                Behaviours[i].StartBehaviour();
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Behaviours[currentState].IsEnd || Behaviours[currentState].CouldBeOverride())
        {
            for (int i = 0; i < numberOfBehaviour; i++)
            {
                if (Behaviours[i].IsValid())
                {
                    if(i == currentState)
                    {
                        break;
                    }
                    else
                    {
                        if (Behaviours[currentState].CouldBeOverride())
                        {
                            Behaviours[currentState].End();
                        }
                        currentState = i;
                        Behaviours[i].StartBehaviour();
                        break;
                    }
                }
            }
        }
        //Fixed the Y position
        this.transform.position = new Vector3(this.transform.position.x, yValue, this.transform.position.z);
        //Lets fix the rotation
        Quaternion rotation = Quaternion.Euler(Vector3.zero);
        this.transform.rotation = rotation;
    }
    public AIStateBaseNode GetBehaviourByEnum(AIBehaviourType type)
    {
       if(type == AIBehaviourType.EAT)
        {
            return this.GetComponent<AIStateEat>();
        }
       else if(type == AIBehaviourType.WALK)
        {
            return this.GetComponent<AIStateWalk>();
        }
       else if(type == AIBehaviourType.IDLE)
        {
            return this.GetComponent<AIStateIdle>();
        }
       else
        {
            return this.GetComponent<AIStateFollowPlayer>();
        }
    }
    public void UpdateAngryLevel(float delta)
    {
        AngryLevel += delta;
        AngryLevel = Mathf.Clamp(AngryLevel, 0, 100);
        AngryLevelBar.value = AngryLevel / 100;
    }

    public float GetAngryLevel()
    {
        return AngryLevel;
    }
}
