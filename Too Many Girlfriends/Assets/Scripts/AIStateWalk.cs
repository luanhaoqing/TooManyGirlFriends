using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is random walk
public class AIStateWalk : AIStateBaseNode
{
    public float Speed;
    private GameObject Player;
    private Transform currentPosition;
    private GameObject aiPlayer;
    public bool shouldChangeDirection;
    private float timer;
    public bool hitTheWall;
    RaycastHit hitPoint;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        aiPlayer = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.IsActive())
        {
            //raycast
            RaycastHit hit;
            if (Physics.Raycast(this.transform.position, this.transform.forward, out hit))
            {
                if ((hit.transform.tag == "Wall" || hit.transform.tag == "AIPlayer") && hit.distance <= 3)
                {
                    shouldChangeDirection = true;
                    hitTheWall = true;
                    hitPoint = hit;
                }
            }
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                shouldChangeDirection = true;
            }
            if(shouldChangeDirection)
            {
                int min = -30;
                int max = 30;
                if (hitTheWall)
                {
                    Vector3 curDir = this.transform.forward;
                    Vector3 newDir = Vector3.Reflect(curDir, hitPoint.normal);
                    newDir.y = curDir.y;
                    float angle = Mathf.DeltaAngle(Mathf.Atan2(curDir.z, curDir.x) * Mathf.Rad2Deg,
                        Mathf.Atan2(newDir.z, newDir.x) * Mathf.Rad2Deg);
                    angle += Random.Range(-45, 45);
                    this.transform.Rotate(new Vector3(0, -angle, 0));
                }
                else
                {
                    this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y + Random.Range(min, max), this.transform.localEulerAngles.z);
                }
                timer = Random.Range(5, 10);
                shouldChangeDirection = false;
                hitTheWall = false;
            }
            this.GetComponent<Rigidbody>().velocity = transform.forward * Speed;
        }
    }
  
    public override bool IsValid()
    {
        if(this.GetComponent<AIBehaviour>().GetAngryLevel() == 100 )
        {
            return true;
        }
        return false;
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
        this.transform.LookAt(Player.transform.position);
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
        if (collision.collider.tag == "Wall" || collision.collider.tag == "AIPlayer")
        {
            shouldChangeDirection = true;
            hitTheWall = true;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall"|| other.tag == "AIPlayer")
        {
            shouldChangeDirection = true;
            hitTheWall = true;
        }
    }

}