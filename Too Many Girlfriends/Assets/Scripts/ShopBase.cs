using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class ShopBase : MonoBehaviour
{
    public int ProgressTime;
    public int PrepareTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int GetProgressTime()
    {
        return ProgressTime;
    }
    public int GetPrepareTime()
    {
        return PrepareTime;
    }
}
