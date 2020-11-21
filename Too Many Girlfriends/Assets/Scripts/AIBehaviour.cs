using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AIBehaviourType
{
    WALK = 0,
    EAT = 1,
}

public class AIBehaviour : MonoBehaviour
{
    public AIBehaviourType[] BehaviourTypeIncluded;

    private int numberOfBehaviour;
    private AIStateBaseNode[] Behaviours;
    // Start is called before the first frame update
    void Start()
    {
        numberOfBehaviour = BehaviourTypeIncluded.Length;
        Behaviours = new AIStateBaseNode[numberOfBehaviour];

        for (int i =0; i<numberOfBehaviour; i++)
        {
            Behaviours[i] = GetBehaviourByEnum(BehaviourTypeIncluded[i]);
        }
        //only for testing
        Behaviours[1].StartBehaviour();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public AIStateBaseNode GetBehaviourByEnum(AIBehaviourType type)
    {
       if(type == AIBehaviourType.EAT)
        {
            return this.GetComponent<AIStateEat>();
        }
       else
        {
            return this.GetComponent<AIStateWalk>();
        }
    }
}
