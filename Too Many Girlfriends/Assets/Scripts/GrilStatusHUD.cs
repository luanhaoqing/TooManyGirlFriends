using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrilStatusHUD : MonoBehaviour
{
    public GameObject Angry;
    public GameObject AIPlayer;
    public GameObject FinalGoal;
    public GameObject TaskPic;
    public GameObject ProgressBar;
    private bool AIPlayerActivated;
    // Start is called before the first frame update
    void Start()
    {
        AIPlayerActivated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(AIPlayer.GetComponent<GoalSystem>().IsInit)
        {
            if(!AIPlayerActivated)
            {
                AIPlayerActivated = true;
                this.transform.Find("Portrait").gameObject.SetActive(true);
                this.transform.Find("Info").gameObject.SetActive(true);
            }
            UpdateAngryLevel(AIPlayer.GetComponent<AIBehaviour>().GetAngryLevel());
            UpdateFinalGoalText(AIPlayer.GetComponent<GoalSystem>().GetFinalGoalText());
            UpdateCurrentTask(AIPlayer.GetComponent<ThoughtBubble>().GetCurrentGoalSprite());
            UpdateProgressBar(AIPlayer.GetComponent<AIBehaviour>().GetCurrentProgressLevel());
            this.transform.Find("Portrait").GetComponentInChildren<RawImage>().texture = AIPlayer.GetComponent<AIBehaviour>().Portraint;
        }
        else
        {
            this.transform.Find("Portrait").gameObject.SetActive(false);
            this.transform.Find("Info").gameObject.SetActive(false);
        }
    }

    public void UpdateAngryLevel(float angryLevel)
    {
        Vector3 currentScale = Angry.GetComponent<Image>().transform.localScale;
        currentScale.y = angryLevel / 100;
        Angry.GetComponent<Image>().transform.localScale = currentScale;
    }
    public void UpdateFinalGoalText(string text)
    {
        FinalGoal.GetComponent<Text>().text = text;
    }
    public void UpdateCurrentTask(Sprite sprite)
    {
        TaskPic.GetComponent<Image>().sprite = sprite;
    }
    public void UpdateProgressBar(float progress)
    {
        ProgressBar.GetComponent<Image>().fillAmount = progress;
    }
}
