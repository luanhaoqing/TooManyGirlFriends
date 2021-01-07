using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateWalkNPC : AIStateBaseNode
{
    public float Speed;
    private Transform currentPosition;
    private GameObject aiPlayer;
    private bool shouldChangeDirection;
    private float timer;
    private bool hitTheWall;
    private bool closeToAIPlayer;
    ContactPoint hitPoint;
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
                    angle += Random.Range(-45, 45);
                    this.transform.Rotate(new Vector3(0,-angle,0));
                }
                else if(closeToAIPlayer)
                {
                    this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y + Random.Range(90, 270), this.transform.localEulerAngles.z);
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
        this.PrintToScreen("WALK AROUND STATE START");
    }
    public override void End(bool sucess = true)
    {
        Progress = 0;
        isActive = false;
        IsEnd = true;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.StopWalking();
        this.PrintToScreen("WALK AROUND STATE END");
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
            hitPoint = collision.contacts[0];
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AIPlayer")
        {
            shouldChangeDirection = true;
            closeToAIPlayer = true;
        }
    }
}
