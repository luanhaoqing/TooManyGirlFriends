using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AIBehaviourType
{
    IDLE = 0,
    WALK = 1,
    EAT = 2,
    FOLLOWPLAYER = 3,
    SHOPPING = 4,
    CATCHPLAYER = 5,
}

public enum PlayerName
{
    CUI_HUA = 0,
    JING_JING = 1,
    LI_JIANGANG = 2,
    SUN_XIAOMEI = 3,
    LIN_MEIMEI = 4,
}
public class AIBehaviour : MonoBehaviour
{
    public AIBehaviourType[] BehaviourTypeIncluded;
    public Slider AngryLevelBar;
    public PlayerName PlayerName;
    private float AngryLevel;
    private int numberOfBehaviour;
    private AIStateBaseNode[] Behaviours;
    private int currentState;
    private float yValue;
    public bool IsTaskSuccess;

    public AIBehaviourType GetCurrentBehaviourType()
    {
        return BehaviourTypeIncluded[currentState];
    }

    // Start is called before the first frame update
    void Start()
    {
        IsTaskSuccess = false;
        AngryLevel = 0;
        yValue = this.transform.position.y;
        numberOfBehaviour = BehaviourTypeIncluded.Length;
        Behaviours = new AIStateBaseNode[numberOfBehaviour];

        for (int i =0; i<numberOfBehaviour; i++)
        {
            Behaviours[i] = GetBehaviourByEnum(BehaviourTypeIncluded[i]);
        }
        for (int i = 0; i < numberOfBehaviour; i++)
        {
            if (Behaviours[i].IsValid())
            {
                currentState = i;
                Behaviours[i].StartBehaviour();
                break;
            }
        }
        this.GetComponent<AIStateIdle>().PrintToScreen("GameText/GameStartText");
    }

    // Update is called once per frame
    void Update()
    {
        if(IsTaskSuccess)
        {
            GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>().HandleAIPlayerSuccess();
            this.gameObject.transform.parent.gameObject.SetActive(false);
        }
        if(Behaviours[currentState].IsEnd || Behaviours[currentState].CouldBeOverride())
        {
            for (int i = 0; i < numberOfBehaviour; i++)
            {
                if (Behaviours[i].IsValid())
                {
                    if(i == currentState)
                    {
                        break;
                    }
                    else
                    {
                        if (Behaviours[currentState].CouldBeOverride())
                        {
                            Behaviours[currentState].End();
                        }
                        currentState = i;
                        Behaviours[i].StartBehaviour();
                        break;
                    }
                }
            }
        }
    }
    public AIStateBaseNode GetBehaviourByEnum(AIBehaviourType type)
    {
       if(type == AIBehaviourType.EAT)
        {
            return this.GetComponent<AIStateEat>();
        }
       else if(type == AIBehaviourType.WALK)
        {
            return this.GetComponent<AIStateWalk>();
        }
       else if(type == AIBehaviourType.IDLE)
        {
            return this.GetComponent<AIStateIdle>();
        }
       else if(type == AIBehaviourType.SHOPPING)
        {
            return this.GetComponent<AIStateShopping>();
        }
       else if(type == AIBehaviourType.FOLLOWPLAYER)
        {
            return this.GetComponent<AIStateFollowPlayer>();
        }
       else
        {
            return this.GetComponent<AIStateCatchPlayer>();
        }
    }
    public void UpdateAngryLevel(float delta)
    {
        AngryLevel += delta;
        AngryLevel = Mathf.Clamp(AngryLevel, 0, 100);
        AngryLevelBar.value = AngryLevel / 100;
        if(AngryLevel <= 0)
        {
            AngryLevelBar.gameObject.SetActive(false);
        }
        else
        {
            AngryLevelBar.gameObject.SetActive(true);
        }
    }

    public float GetAngryLevel()
    {
        return AngryLevel;
    }

    public float GetCurrentProgressLevel()
    {
        AIBehaviourType type = GetCurrentBehaviourType();
        if(type == AIBehaviourType.EAT || type == AIBehaviourType.SHOPPING)
        {
            return Behaviours[currentState].Progress;
        }
        return 0;
    }

    public string GetPlayerName()
    {
        switch(PlayerName)
        {
            case PlayerName.CUI_HUA:
                return I2.Loc.LocalizationManager.GetTranslation("PlayerName/GirlFriendName_Cuihua");
            case PlayerName.JING_JING:
                return I2.Loc.LocalizationManager.GetTranslation("PlayerName/GirlFriendName_JingJing");
            case PlayerName.LIN_MEIMEI:
                return I2.Loc.LocalizationManager.GetTranslation("PlayerName/GirlFriendName_Linmeimei");
            case PlayerName.LI_JIANGANG:
                return I2.Loc.LocalizationManager.GetTranslation("PlayerName/GirlFriendName_Lijiangang");
            case PlayerName.SUN_XIAOMEI:
                return I2.Loc.LocalizationManager.GetTranslation("PlayerName/GirlFriendName_Sunxiaomei");
            default:
                return null;
        }
    }
}
