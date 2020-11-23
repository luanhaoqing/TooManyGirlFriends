using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateIdle : AIStateBaseNode
{
    public float AngryLevelMaxTime;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (this.IsActive())
        {
            this.GetComponent<AIBehaviour>().UpdateAngryLevel((Time.deltaTime / AngryLevelMaxTime) * 100);
        }
    }

    public override bool IsValid()
    {
        //Need some detection here for entering this node
        return true;
    }
    public override void StartBehaviour()
    {
        Progress = 0;
        IsEnd = false;
        isActive = true;
    }
    public override void End()
    {
        Progress = 0;
        IsEnd = true;
        isActive = false;
    }
    public override bool CouldBeOverride()
    {
        return true;
    }
}
