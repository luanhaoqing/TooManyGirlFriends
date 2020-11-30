using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is random walk
public class AIStateWalk : AIStateBaseNode
{
    public float Speed;
    public GameObject Player;
    private Transform currentPosition;
    private GameObject aiPlayer;
    private bool shouldChangeDirection;
    private float timer;
    private bool hitTheWall;
    // Start is called before the first frame update
    void Start()
    {
        aiPlayer = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.IsActive())
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                shouldChangeDirection = true;
            }
            if(shouldChangeDirection)
            {
                int min = hitTheWall ? 90 : -45;
                int max = hitTheWall ? 180 : 45;
                this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y + Random.Range(min, max), this.transform.localEulerAngles.z);
                this.GetComponent<Rigidbody>().velocity = transform.forward * Speed;
                timer = Random.Range(1, 5);
                shouldChangeDirection = false;
                hitTheWall = false;
            }
        }
    }
  
    public override bool IsValid()
    {
        if(this.GetComponent<AIBehaviour>().GetAngryLevel() == 100)
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
        timer = Random.Range(1, 5);
        this.transform.LookAt(Player.transform.position);
        this.GetComponent<Rigidbody>().velocity = transform.forward * Speed;
        this.StartWalking();
    }
    public override void End()
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


}