using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIStateFollowPlayer : AIStateBaseNode
{
    public float Speed;
    public GameObject MyPlayer;
    public float AngryLevelReduceToMinTime;
    public bool ForceFollowPlayerState;
    public float DistanceToNoticePlayer;
    public float DistanceToSeePlayer;

    private Transform currentPosition;
    private GameObject aiPlayer;
    private NavMeshAgent man;
    // Start is called before the first frame update
    void Start()
    {
        aiPlayer = this.gameObject;
        ForceFollowPlayerState = false;
        man = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.IsActive())
        {
            float distance = Mathf.Clamp(GetDistance(MyPlayer, aiPlayer), 3, 5);
            float currentSpeed = Mathf.Lerp(0, Speed, (distance - 3) / 2);
            //walk state will only exit if other state is available 
            if(currentSpeed>0.25f)
            {
                // aiPlayer.transform.position = Vector3.MoveTowards(aiPlayer.transform.position, MyPlayer.transform.position, currentSpeed * Time.deltaTime);
                man.SetDestination(MyPlayer.transform.position);
                man.speed = currentSpeed;
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
    public override bool IsValid()
    {
        if(this.GetComponent<AIBehaviour>().GetCurrentBehaviourType() == AIBehaviourType.IDLE || this.GetComponent<AIBehaviour>().GetCurrentBehaviourType() == AIBehaviourType.WALK)
        {
            aiPlayer = this.gameObject;
            //If player is close to AI Player
            if (GetDistance(MyPlayer, aiPlayer) < DistanceToNoticePlayer || couldSeePlayer() || ForceFollowPlayerState) return true;
        }
        else if(this.GetComponent<AIBehaviour>().GetCurrentBehaviourType() == AIBehaviourType.FOLLOWPLAYER)
        {
            return true;
        }
        return false;
    }
    public override void StartBehaviour()
    {
        isActive = true;
        //This Follow Player Stats could be End anytime
        IsEnd = false;
        this.PrintToScreen("FOLLOW PLAYER STATE START");
        ShowBubble();
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
    private bool couldSeePlayer()
    {
        Vector3 tempVec = MyPlayer.transform.position - this.gameObject.transform.position;
        tempVec = Vector3.Normalize(tempVec);
        Vector3 facing = Vector3.Normalize(this.gameObject.transform.forward);
        float cosVlaue = (tempVec.x * facing.x + tempVec.y * facing.y + tempVec.z * facing.z);
        if((cosVlaue)>=0.86f && GetDistance(MyPlayer, this.gameObject) <= DistanceToSeePlayer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
