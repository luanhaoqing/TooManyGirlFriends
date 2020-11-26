using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is random walk
public class AIStateWalk : AIStateBaseNode
{
    public float Speed;
    public GameObject Player;
    //well, this should never be completed ever
    private int CompletionTime = 9999999;
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
        if(this.IsActive())
        {
            //walk state will only exit if other state is available 
            Progress += Time.deltaTime / CompletionTime;
           
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

        this.transform.LookAt(Player.transform.position);
        this.GetComponent<Rigidbody>().velocity = transform.forward * Speed;
    }
    public override void End()
    {
        Progress = 0;
        isActive = false;
        IsEnd = true;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
    public override bool CouldBeOverride()
    {
        return true;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Wall")
        {
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }


}