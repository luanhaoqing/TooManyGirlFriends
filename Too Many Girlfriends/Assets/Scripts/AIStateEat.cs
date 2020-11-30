using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIStateEat : AIStateBaseNode
{
    public int CompletionTime = 10;
    public GameObject ActionBar;
    public GameObject ActionButton;
    private bool isValid;
    private bool hasEverFinished;
    // Start is called before the first frame update
    void Start()
    {
        isValid = false;
        hasEverFinished = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.IsActive())
        {
            Progress += Time.deltaTime / CompletionTime;
            ActionBar.GetComponent<Slider>().value = Progress;
            if (Progress >= 1)
            {
                this.End();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Restaurant" && !hasEverFinished && this.GetComponent<AIBehaviour>().GetCurrentBehaviourType()== AIBehaviourType.FOLLOWPLAYER)
        {
            ActionButton.SetActive(true);
            Button btn = ActionButton.GetComponent<Button>();
            btn.GetComponentInChildren<Text>().text = "START EAT";
            btn.onClick.AddListener(TaskOnClick);
        }
    }

    void TaskOnClick()
    {
        isValid = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Restaurant" && !hasEverFinished)
        {
            isValid = false;
            ActionButton.SetActive(false);
        }
    }

    public override bool IsValid()
    {
        //Need some detection here for entering this node
        if (hasEverFinished) return false;

        return isValid;
    }
    public override void StartBehaviour()
    {
        Progress = 0;
        ActionBar.SetActive(true);
        IsEnd = false;
        isActive = true;
        this.PrintToScreen("EAT STATE START");
    }
    public override void End()
    {
        Progress = 0;
        ActionBar.SetActive(false);
        IsEnd = true;
        isActive = false;
        hasEverFinished = true;
        ActionButton.SetActive(false);
        this.PrintToScreen("EAT STATE END");
    }
}

