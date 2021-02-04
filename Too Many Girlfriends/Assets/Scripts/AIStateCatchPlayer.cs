using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIStateCatchPlayer : AIStateBaseNode
{
    public float Speed;
    private GameObject MyPlayer;
    public float AngryLevelReduceToMinTime;
    public float DistanceToSeePlayer;

    private Transform currentPosition;
    private GameObject aiPlayer;
    private NavMeshAgent man;
    private Vector3 lastSeenPlayerPos;
    private bool isInWalk;
    // Start is called before the first frame update
    void Start()
    {
        isInWalk = false;
        MyPlayer = GameObject.FindGameObjectWithTag("Player");
        aiPlayer = this.gameObject;
        man = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.IsActive())
        {
            //update last seen player position if could see player.
            if(couldSeePlayer())
            {
                man.SetDestination(lastSeenPlayerPos);
                man.speed = Speed;
                this.StartWalking();
                isInWalk = true;
            }

           if(Vector3.Distance(this.transform.position,lastSeenPlayerPos)<=3.1)
            {
                aiPlayer.GetComponent<Rigidbody>().velocity = Vector3.zero;
                this.StopWalking();
                this.End();
                isInWalk = false;
            }
            this.GetComponent<AIBehaviour>().UpdateAngryLevel((Time.deltaTime / AngryLevelReduceToMinTime) * -100);
            this.transform.LookAt(lastSeenPlayerPos);
        }
    }
    public override bool IsValid()
    {
        if(MyPlayer == null)
        {
            MyPlayer = GameObject.FindGameObjectWithTag("Player");
        }
        else if (MyPlayer.GetComponent<PlayerMovement>().IsInRestroom)
        {
            return false;
        }
        if (this.GetComponent<AIBehaviour>().GetCurrentBehaviourType() == AIBehaviourType.IDLE || this.GetComponent<AIBehaviour>().GetCurrentBehaviourType() == AIBehaviourType.WALK)
        {
            aiPlayer = this.gameObject;
            //If player is close to AI Player
            if (couldSeePlayer()) return true;
        }
        else if (isInWalk)
        {
            return true;
        }
        return false;
    }
    public override void StartBehaviour()
    {
        isActive = true;
        IsEnd = false;
        this.GetComponentInChildren<Animator>().SetFloat("Speed",1);
    }
    public override void End(bool sucess = true)
    {
        isActive = false;
        IsEnd = true;
        isInWalk = false;
        this.StopWalking();
        this.GetComponentInChildren<Animator>().SetFloat("Speed", 1);
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

        if ((cosVlaue) >= 0.86f && GetDistance(MyPlayer, this.gameObject) <= DistanceToSeePlayer)
        {
            RaycastHit[] hits;
            hits = Physics.RaycastAll(this.transform.position, tempVec, DistanceToSeePlayer);
            for(int i = 0;i<hits.Length;i++)
            {
                if(hits[i].transform.tag == "Wall")
                {
                    return false;
                }
            }
            lastSeenPlayerPos = MyPlayer.transform.position;
            return true;
        }
        else
        {
            return false;
        }
    }
}
