using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithPlayer : MonoBehaviour
{
    public GameObject AIPlayer;
    private float yValue;
    // Start is called before the first frame update
    void Start()
    {
        yValue = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(AIPlayer.transform.position.x, yValue, AIPlayer.transform.position.z);
    }
}
