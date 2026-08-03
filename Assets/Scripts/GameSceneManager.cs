using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneManager : GameSceneManagerBase
{
    // Gaurav Singh

    // This script manages every scene in Act 1 minus the main menu

    [SerializeField] private AudioClip alarm;
    [SerializeField] private AudioClip boards;

    [SerializeReference]
    ScriptsSO bearIdle, bearInit;

    private bool isAlarmOff = false;

    // there is clearly a better way to do this, not rn
    public ScriptsSO AlarmOn, FrontDoorLock, FrontDoorGoal, InitWindow, leaveWindow, questionWindow, FrontDoorHint;

    protected override void OnEnable()
    {
		base.OnEnable();

        switch (SceneManager.GetActiveScene().name)
        {
            case SceneNames.ACT1_BEDROOM or SceneNames.ACT1_BED_WINDOW:
                if (PDSO.triggers.Contains(TriggerNames.CLOCK))
                {
                    Clock();
                }
				else
				{
					BackgroundSFX.PlayLoop(alarm);
				}
                break;
            case SceneNames.ACT1_KITCHEN:
                if (PDSO.triggers.Contains(TriggerNames.KITCHEN_BOARDS_REMOVED))
                {
                    GameObject.Find("Boards").GetComponent<Image>().enabled = false;
                }
                break;
        }
    }

    // Bedroom & Bed Window Scene 
    public void OnClockClick()
    {
        switch (PDSO.HeldItem)
        {
            case "":
                if (!isAlarmOff)
                {
                    Clock();
                    PDSO.triggers.Add(TriggerNames.CLOCK);
                }
                break;
            default:
                DM.SetLines(DefaultItemFail);
                break;
        }
			
    }

    public void Clock()
    {
        isAlarmOff = true;
		BackgroundSFX.StopLoop();
    }

    // Bedroom Scene
    public void BedroomWindow()
    {
        IM.HoldItem("");

        NextScene(SceneNames.ACT1_BED_WINDOW);
    }

    public void ExitBedroom()
    {
        if (!isAlarmOff)
        {
            DM.SetLines(AlarmOn);
            return;
        }

		MoveSFX.Play();
        NextScene(SceneNames.ACT1_LIVING_ROOM);
    }

    // Bed Window Scene
    public void ExitWindow()
    {
        NextScene(SceneNames.ACT1_BEDROOM);
    }

    // Living Room Scene
    public void FrontDoor()
    {
        switch (PDSO.HeldItem)
        {
            case "":
                if (PDSO.triggers.Contains(TriggerNames.GOAL_HEARD))
                {
                    DM.SetLines(FrontDoorGoal);
                }
                else
                {
                    DM.SetLines(FrontDoorLock);
                }
                break;
            default:
                DM.SetLines(DefaultItemFail);
                break;
        }
    }

    public void Closet(bool open)
    {
        switch (PDSO.HeldItem)
        {
            case "":
                GameObject.Find("Closet Closed").GetComponent<Image>().enabled = !open;
                GameObject.Find("Closet Open").GetComponent<Image>().enabled = open;
                GameObject.Find("Closet Open").transform.Find("Close Open Hitbox").gameObject.SetActive(open);
                MoveSFX.Play();
                break;
            default:
                DM.SetLines(DefaultItemFail);
                break;
        }
    }

    public void BearDia()
    {
        switch (PDSO.HeldItem)
        {
            case "":
                if (PDSO.triggers.Contains(TriggerNames.GOAL_HEARD))
                {
                    DM.SetLines(bearIdle);
                }
                else
                {
                    DM.SetLines(bearInit);
                }
                break;
            default:
                DM.SetLines(DefaultItemFail);
                break;
        }
    }

    public void LRtoBR()
    {
		MoveSFX.Play();
        NextScene(SceneNames.ACT1_BEDROOM);
    }

    public void LRtoK()
    {
        NextScene(SceneNames.ACT1_KITCHEN);
    }

    // Kitchen Scene
    public void KtoLR()
    {
        NextScene(SceneNames.ACT1_LIVING_ROOM);
    }

    public void BoardedUpWindows()
    {
        switch (PDSO.HeldItem)
        {
            case "":
                if (PDSO.triggers.Contains(TriggerNames.KITCHEN_BOARDS_REMOVED))
                {
                    DM.SetLines(leaveWindow);
                }
                else if(PDSO.triggers.Contains(TriggerNames.GOAL_HEARD) && PDSO.triggers.Contains(TriggerNames.FRONT_DOOR))
                {
                    DM.SetLines(questionWindow);
                }
                else
                {
                    DM.SetLines(InitWindow);
                }
                break;
            case "Crowbar":
                if (PDSO.triggers.Contains(TriggerNames.GOAL_HEARD) && PDSO.triggers.Contains(TriggerNames.FRONT_DOOR))
                {
                    GameObject.Find("Boards").GetComponent<Image>().enabled = false;
                    PDSO.triggers.Add(TriggerNames.KITCHEN_BOARDS_REMOVED);

                    IM.HoldItem("");
					BackgroundSFX.PlayClip(boards);
                }
                else
                {
                    DM.SetLines(FrontDoorHint);
                }
                break;
            default:
                DM.SetLines(DefaultItemFail);
                break;
        }
    }

    

}
