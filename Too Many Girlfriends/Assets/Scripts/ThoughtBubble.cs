using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoughtBubble : MonoBehaviour
{
    public GameObject Bubble;
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowBubble(BubbleType type)
    {
        Bubble.SetActive(true);
        Bubble.GetComponent<SpriteRenderer>().sprite = Sprites[(int)type];
    }
    public void HideBubble()
    {
        Bubble.SetActive(false);
    }
}
