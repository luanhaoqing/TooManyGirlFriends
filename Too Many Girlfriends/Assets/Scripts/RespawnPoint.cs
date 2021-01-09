using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{

    public void GenerateNPC(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, this.transform.position, Quaternion.identity);
        obj.transform.forward = this.transform.forward;
        obj.GetComponent<AIStateWalkNPC>().Speed += Random.Range(-2, 2);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
