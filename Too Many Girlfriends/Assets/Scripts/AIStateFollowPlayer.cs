using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateFollowPlayer : AIStateBaseNode
{
    public float Speed;
    public GameObject MyPlayer;
    public float AngryLevelReduceToMinTime;

    private Transform currentPosition;
    private GameObject aiPlayer;
    // Start is called before the first frame update
    void Start()
    {
        aiPlayer = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.IsActive())
        {
            //walk state will only exit if other state is available 
            if(getDistance(MyPlayer,aiPlayer)>3f)
            {
                aiPlayer.transform.position = Vector3.MoveTowards(aiPlayer.transform.position, MyPlayer.transform.position, Speed * Time.deltaTime);
                this.StartWalking();
            }
            else
            {
                aiPlayer.GetComponent<Rigidbody>().velocity = Vector3.zero;
                this.StopWalking();
            }
            this.GetComponent<AIBehaviour>().UpdateAngryLevel((Time.deltaTime / AngryLevelReduceToMinTime) * -100);
            this.transform.LookAt(MyPlayer.transform.position);
        }
    }

    private float getDistance(GameObject AIPlayer, GameObject MyPlayer)
    {
        Vector3 AIPlayerPos = AIPlayer.transform.position;
        Vector3 MyPlayerPos = MyPlayer.transform.position;
        return Vector3.Distance(AIPlayerPos, MyPlayerPos);
    }
    public override bool IsValid()
    {
        aiPlayer = this.gameObject;
        //If player is close to AI Player
        if (getDistance(MyPlayer, aiPlayer) < 5) return true;
        else return false;
    }
    public override void StartBehaviour()
    {
        isActive = true;
        //This Follow Player Stats could be End anytime
        IsEnd = false;
        this.PrintToScreen("FOLLOW PLAYER STATE START");
    }
    public override void End()
    {
        isActive = false;
        IsEnd = true;
        this.StopWalking();
        this.PrintToScreen("FOLLOW PLAYER STATE END");
    }
    public override bool CouldBeOverride()
    {
        return true;
    }
}
