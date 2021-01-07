using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGenerator : MonoBehaviour
{
    public GameObject NPCPrefab;
    public int MaxNPCNumber;
    public GameObject[] InitPositions;
    public float InitInterval;
    private float timer;
    private int currentNPCNum = 0;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= InitInterval && currentNPCNum<MaxNPCNumber)
        {
            currentNPCNum++;
            timer = 0;
            int randomPos = Random.Range(0, InitPositions.Length - 1);
            Instantiate(NPCPrefab, InitPositions[randomPos].transform.position,Quaternion.identity);
        }
    }
}
