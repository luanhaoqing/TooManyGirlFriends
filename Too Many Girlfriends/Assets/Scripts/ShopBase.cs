using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class ShopBase : MonoBehaviour
{
    public int ProgressTime;
    public int PrepareTime;
    private GameObject bubble;
    private GameObject mplayer;
    private bool isShowingBubble;
    private IEnumerator func;
    // Start is called before the first frame update
    void Start()
    {
        bubble = this.transform.Find("Thought Bubble").gameObject;
        mplayer = GameObject.FindGameObjectWithTag("Player");
        isShowingBubble = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(mplayer.transform.position,this.transform.position)<=10)
        {
            if(!isShowingBubble)
            {
                if(func!=null)
                {
                    StopCoroutine(func);
                }
                ShowBubble();
                isShowingBubble = true;
            }
        }
        else
        {
            if(isShowingBubble)
            {
                func = WaitAndHideBubble();
                StartCoroutine(func);
                isShowingBubble = false;
            }
        }
    }
    public int GetProgressTime()
    {
        return ProgressTime;
    }
    public int GetPrepareTime()
    {
        return PrepareTime;
    }
    public void ShowBubble()
    {
        bubble.SetActive(true);
        bubble.GetComponent<Animator>().SetTrigger("ShowBubble");
    }
    public void HideBubble()
    {
        bubble.SetActive(false);
    }
    IEnumerator WaitAndHideBubble()
    {
        yield return new WaitForSeconds(0.5f);
        HideBubble();
    }
}
