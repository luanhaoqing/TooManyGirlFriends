using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isSuccess = false;
    private bool isFailed = false;
    private int totalAIPlayerNum;
    private GameObject[] AIPlayers;
    public int SuccessedAIPlayerCount;
    public GameObject GameResultOverlay;
    void Start()
    {
        SuccessedAIPlayerCount = 0;
        totalAIPlayerNum = GameObject.FindGameObjectsWithTag("AIPlayer").Length;
        AIPlayers = new GameObject[totalAIPlayerNum];
        AIPlayers = GameObject.FindGameObjectsWithTag("AIPlayer");
    }

    // Update is called once per frame
    void Update()
    {
       if(!isFailed && totalAIPlayerNum == SuccessedAIPlayerCount)
        {
            isSuccess = true;
            GameResultOverlay.GetComponent<Text>().text = "YOU WIN!";
        }
       else if(isAIPlayerMeetEachother())
        {
            isFailed = true;
            GameResultOverlay.GetComponent<Text>().text = "YOU LOSE!";
        }
       if(isFailed||isSuccess)
        {
            GameResultOverlay.SetActive(true);
        }
    }
    public void HandleAIPlayerSuccess()
    {
        SuccessedAIPlayerCount += 1;
    }
    private bool isAIPlayerMeetEachother()
    {
        bool isInFollowPlayerState = true;
        for(int i = 0;i<totalAIPlayerNum;i++)
        {
            if(AIPlayers[i].GetComponent<AIBehaviour>().GetCurrentBehaviourType() != AIBehaviourType.FOLLOWPLAYER)
            {
                isInFollowPlayerState = false;
            }
        }
        return isInFollowPlayerState;
    }
}
