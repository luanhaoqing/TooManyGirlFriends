using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    public GameObject WinObj;
    public GameObject LoseObj;
    // Start is called before the first frame update
    void Start()
    {
        if(GameMaster.IsWin)
        {
            WinObj.SetActive(true);
            LoseObj.SetActive(false);
        }
        else
        {
            WinObj.SetActive(false);
            LoseObj.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
