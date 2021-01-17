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
    private int SuccessedAIPlayerCount;
    private float refreshTimer;
    private int currentGirlFriendNum;

    public GameObject GameResultOverlay;
    public GameObject InitalPoint;
    public GameObject[] GirlFriends;
    public float RefreshTimeUpperLevel;
    public float RefreshTimeLowerLevel;
    void Start()
    {
        SuccessedAIPlayerCount = 0;
        totalAIPlayerNum = GirlFriends.Length;
        currentGirlFriendNum = 0;
        refreshTimer = Random.Range(RefreshTimeLowerLevel, RefreshTimeUpperLevel);
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

        //Refresh Girl Friends
        if(currentGirlFriendNum<totalAIPlayerNum)
        {
            refreshTimer -= Time.deltaTime;
            if(refreshTimer<=0)
            {
                showNewGirlFriend();
            }
        }
    }
    private void showNewGirlFriend()
    {
        GirlFriends[currentGirlFriendNum].transform.position = InitalPoint.transform.position;
        GirlFriends[currentGirlFriendNum].SetActive(true);
        currentGirlFriendNum++;
        refreshTimer = Random.Range(RefreshTimeLowerLevel, RefreshTimeUpperLevel);
    }
    public void HandleAIPlayerSuccess()
    {
        SuccessedAIPlayerCount += 1;
    }
    private bool isAIPlayerMeetEachother()
    {
        int count = 0;
        for(int i = 0;i<totalAIPlayerNum;i++)
        {
            if(GirlFriends[i].activeSelf && GirlFriends[i].GetComponentInChildren<AIBehaviour>().GetCurrentBehaviourType() == AIBehaviourType.FOLLOWPLAYER)
            {
                count++;
            }
        }
        return count>=2;
    }
}
