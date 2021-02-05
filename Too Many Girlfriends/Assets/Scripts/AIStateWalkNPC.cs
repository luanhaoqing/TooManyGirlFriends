using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIStateWalkNPC : AIStateBaseNode
{
    public float Speed;
    private Transform currentPosition;
    private GameObject aiPlayer;
    private bool shouldChangeDirection;
    private float timer;
    private bool hitTheWall;
    private bool closeToAIPlayer;
    NavMeshHit hitPoint;
    // Start is called before the first frame update
    void Start()
    {
        aiPlayer = this.gameObject;
        this.StartBehaviour();
        hitTheWall = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.IsActive())
        {
            //raycast
            // RaycastHit hit;
            // if(Physics.Raycast(this.transform.position,this.transform.forward,out hit))
            // {
            //     if(hit.transform.tag == "Wall" && hit.distance <= 2)
            //     {
            //         shouldChangeDirection = true;
            //         hitTheWall = true;
            //         hitPoint = hit;
            //
            //         //debug line draw
            //        // Debug.DrawLine(this.transform.position, hit.point, Color.red);
            //        // Debug.DrawRay(hit.point, Vector3.Reflect(this.transform.forward, hitPoint.normal), Color.green);
            //     }
            // }
            NavMeshHit hit;
            Vector3 targetPos = this.transform.forward + this.transform.position;
            if (NavMesh.Raycast(transform.position, targetPos, out hit, NavMesh.AllAreas))
            {
                shouldChangeDirection = true;
                hitTheWall = true;
                hitPoint = hit;
            }
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                shouldChangeDirection = true;
            }
            if (shouldChangeDirection)
            {
                int min = -30;
                int max = 30;
                if(hitTheWall)
                {
                    Vector3 curDir = this.transform.forward;
                    Vector3 newDir = Vector3.Reflect(curDir, hitPoint.normal);
                    newDir.y = curDir.y;
                    float angle = Mathf.DeltaAngle(Mathf.Atan2(curDir.z, curDir.x) * Mathf.Rad2Deg,
                        Mathf.Atan2(newDir.z, newDir.x) * Mathf.Rad2Deg);
                    angle += Random.Range(-15, 15);
                    this.transform.Rotate(new Vector3(0,-angle,0));
                }
                else
                {
                    this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y + Random.Range(min, max), this.transform.localEulerAngles.z);
                }
                this.GetComponent<Rigidbody>().velocity = transform.forward * Speed;
                timer = Random.Range(5, 10);
                shouldChangeDirection = false;
                hitTheWall = false;
            }
        }
    }

    public override bool IsValid()
    {
        return true;
    }
    public override void StartBehaviour()
    {
        Progress = 0;
        isActive = true;
        IsEnd = false;
        shouldChangeDirection = false;
        hitTheWall = false;
        this.GetComponentInChildren<Animator>().SetFloat("Speed", 1);
        timer = Random.Range(1, 5);
        this.GetComponent<Rigidbody>().velocity = transform.forward * Speed;
        this.StartWalking();
    }
    public override void End(bool sucess = true)
    {
        Progress = 0;
        isActive = false;
        IsEnd = true;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.StopWalking();
    }
    public override bool CouldBeOverride()
    {
        return true;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Wall")
        {
            shouldChangeDirection = true;
            hitTheWall = true;
        }
    }
    void OnTriggerEnter(Collider other)
    {
    }
}
