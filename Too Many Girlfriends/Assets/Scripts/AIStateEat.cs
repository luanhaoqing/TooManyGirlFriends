using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateEat : AIStateBaseNode
{
    public int CompletionTime = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.IsActive())
        {
            Progress += Time.deltaTime / CompletionTime;
            if (Progress >= 1)
            {
                this.End();
            }
        }
    }

    public override bool IsValid()
    {
        //Need some detection here for entering this node
        return false;
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
}

