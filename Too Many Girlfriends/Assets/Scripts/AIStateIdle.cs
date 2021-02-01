using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateIdle : AIStateBaseNode
{
    public float AngryLevelMaxTime;
    public float MinRotationTime;
    public float MaxRotationTime;
    public bool FinishedTask;
    public float BonusTimerForTaskCompletion;
    public float BonusTimerInit;
    private bool isInit;
    private float bonusTimer;
    private float rotationTimer = 0;
    private float currentRotationWaitTimer;
    // Start is called before the first frame update
    void Start()
    {
        rotationTimer = 0;
        bonusTimer = 0;
        currentRotationWaitTimer = Random.Range(MinRotationTime, MaxRotationTime);
        FinishedTask = true;
        isInit = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.IsActive())
        {
            if(FinishedTask)
            {
                bonusTimer += Time.deltaTime;
                if(isInit)
                {
                    if (bonusTimer >= BonusTimerInit)
                    {
                        FinishedTask = false;
                        isInit = false;
                    }
                }
                else if(bonusTimer >= BonusTimerForTaskCompletion)
                {
                    FinishedTask = false;
                }
            }
            else
            {
                this.GetComponent<AIBehaviour>().UpdateAngryLevel((Time.deltaTime / AngryLevelMaxTime) * 100);
            }
            rotationTimer += Time.deltaTime;
            if(rotationTimer >= currentRotationWaitTimer)
            {
                this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y + Random.Range(-180, 180), this.transform.localEulerAngles.z);
                rotationTimer = 0;
                currentRotationWaitTimer = Random.Range(0, MaxRotationTime);
            }
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    public override bool IsValid()
    {
        return true;
    }
    public override void StartBehaviour()
    {
        Progress = 0;
        bonusTimer = 0;
        IsEnd = false;
        isActive = true;
        if(FinishedTask)
        {
            this.PrintToScreen("GameText/IdleText");
        }
    }
    public override void End(bool sucess = true)
    {
        Progress = 0;
        IsEnd = true;
        isActive = false;
        isInit = false;
        FinishedTask = false;
    }
    public override bool CouldBeOverride()
    {
        return true;
    }
}
