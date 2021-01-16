using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThoughtBubble : MonoBehaviour
{
    public GameObject Bubble;
    public GameObject ActivedObj;
    private GameObject ProgressBar;
    public enum BubbleType
    {
        CLOTHING = 0,
        DINNER = 1,
        GRILL = 2,
        ICECREAM = 3,
        KEBAB = 4,
        WASHROOM = 5,
    }
    public Sprite[] Sprites;
    // Start is called before the first frame update
    void Start()
    {
        Bubble.SetActive(false);
        ProgressBar = Bubble.gameObject.transform.Find("Progress").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowBubble()
    {
        ShowBubble(getCurrentBubbleType());
    }
    private void ShowBubble(BubbleType type)
    {
        Bubble.SetActive(true);
        Bubble.gameObject.transform.Find("Content").GetComponent<Image>().sprite = Sprites[(int)type];
        Bubble.GetComponent<Animator>().SetTrigger("ShowBubble");
    }
    private BubbleType getCurrentBubbleType()
    {
        GoalType type = this.GetComponent<GoalSystem>().GetCurrentGoal();
        BubbleType bubbleType = BubbleType.CLOTHING;
        switch (type)
        {
            case GoalType.FLOWER:
                bubbleType = BubbleType.CLOTHING;
                break;
            case GoalType.GRILL:
                bubbleType = BubbleType.GRILL;
                break;
            case GoalType.ICECREAM:
                bubbleType = BubbleType.ICECREAM;
                break;
            case GoalType.KABAB:
                bubbleType = BubbleType.KEBAB;
                break;
          //  case GoalType.RESTAURANT:
          //      bubbleType = BubbleType.DINNER;
           //     break;
            case GoalType.SHOPPING:
                bubbleType = BubbleType.CLOTHING;
                break;
        }
        return bubbleType;
    }
    public void HideBubble()
    {
        Bubble.SetActive(false);
    }
    public Sprite GetCurrentGoalSprite()
    {
        return Sprites[(int)getCurrentBubbleType()];
    }
    public void UpdateProgressBar(float progress)
    {
        ProgressBar.GetComponent<Image>().fillAmount = progress;
    }
    public void HandleTaskStart()
    {
        ActivedObj.SetActive(true);
        UpdateProgressBar(0);
    }
    public void HandleTaskEnd()
    {
        ActivedObj.SetActive(false);
        UpdateProgressBar(0);
    }
}
