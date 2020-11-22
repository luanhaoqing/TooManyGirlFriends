using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateWalk : AIStateBaseNode
{
    public float Speed;
    public GameObject Destination;
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
            aiPlayer.transform.position = Vector3.MoveTowards(aiPlayer.transform.position, Destination.transform.position, Speed * Time.deltaTime);
        }
    }
  
    public override bool IsValid()
    {
        //This will always be valid.
        return true;
    }
    public override void StartBehaviour()
    {
        Progress = 0;
        isActive = true;
        IsEnd = false;
    }
    public override void End()
    {
        Progress = 0;
        isActive = false;
        IsEnd = true;
    }
    public override bool CouldBeOverride()
    {
        return true;
    }
}