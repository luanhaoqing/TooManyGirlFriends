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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAngryLevel(AIPlayer.GetComponent<AIBehaviour>().GetAngryLevel());
        UpdateFinalGoalText(AIPlayer.GetComponent<GoalSystem>().GetFinalGoalText());
        UpdateCurrentTask(AIPlayer.GetComponent<ThoughtBubble>().GetCurrentGoalSprite());
        UpdateProgressBar(AIPlayer.GetComponent<AIBehaviour>().GetCurrentProgressLevel());
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
