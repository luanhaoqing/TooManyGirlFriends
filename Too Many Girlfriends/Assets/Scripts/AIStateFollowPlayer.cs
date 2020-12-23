using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateFollowPlayer : AIStateBaseNode
{
    public float Speed;
    public GameObject MyPlayer;
    public float AngryLevelReduceToMinTime;
    public bool ForceFollowPlayerState;

    private Transform currentPosition;
    private GameObject aiPlayer;
    // Start is called before the first frame update
    void Start()
    {
        aiPlayer = this.gameObject;
        ForceFollowPlayerState = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.IsActive())
        {
            float distance = Mathf.Clamp(getDistance(MyPlayer, aiPlayer), 3, 5);
            float currentSpeed = Mathf.Lerp(0, Speed, (distance - 3) / 2);
            //walk state will only exit if other state is available 
            if(currentSpeed>0.25f)
            {
                aiPlayer.transform.position = Vector3.MoveTowards(aiPlayer.transform.position, MyPlayer.transform.position, currentSpeed * Time.deltaTime);
                this.GetComponentInChildren<Animator>().SetFloat("Speed", currentSpeed / Speed);
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
        if (getDistance(MyPlayer, aiPlayer) < 5 || ForceFollowPlayerState) return true;
        else return false;
    }
    public override void StartBehaviour()
    {
        isActive = true;
        //This Follow Player Stats could be End anytime
        IsEnd = false;
        this.PrintToScreen("FOLLOW PLAYER STATE START");
    }
    public override void End(bool sucess = true)
    {
        isActive = false;
        ForceFollowPlayerState = false;
        IsEnd = true;
        this.StopWalking();
        this.GetComponentInChildren<Animator>().SetFloat("Speed", 1);
        this.PrintToScreen("FOLLOW PLAYER STATE END");
    }
    public override bool CouldBeOverride()
    {
        return true;
    }
}
