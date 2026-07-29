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
            case "A1 Bedroom" or "A1 Bed Window":
                if (PDSO.triggers.Contains("Clock"))
                {
                    Clock();
                }
				else
				{
					BackgroundSFX.PlayLoop(alarm);
				}
                break;
            case "A1 Kitchen":
                if (PDSO.triggers.Contains("Boards Removed"))
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
                    PDSO.triggers.Add("Clock");
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

        NextScene("A1 Bed Window");
    }

    public void ExitBedroom()
    {
        if (!isAlarmOff)
        {
            DM.SetLines(AlarmOn);
            return;
        }

		MoveSFX.Play();
        NextScene("A1 Living Room");
    }

    // Bed Window Scene
    public void ExitWindow()
    {
        NextScene("A1 Bedroom");
    }

    // Living Room Scene
    public void FrontDoor()
    {
        switch (PDSO.HeldItem)
        {
            case "":
                if (PDSO.triggers.Contains("Goal Heard"))
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
                if (PDSO.triggers.Contains("Goal Heard"))
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
        NextScene("A1 Bedroom");
    }

    public void LRtoK()
    {
        NextScene("A1 Kitchen");
    }

    // Kitchen Scene
    public void KtoLR()
    {
        NextScene("A1 Living Room");
    }

    public void BoardedUpWindows()
    {
        switch (PDSO.HeldItem)
        {
            case "":
                if (PDSO.triggers.Contains("Boards Removed"))
                {
                    DM.SetLines(leaveWindow);
                }
                else if(PDSO.triggers.Contains("Goal Heard") && PDSO.triggers.Contains("Front Door"))
                {
                    DM.SetLines(questionWindow);
                }
                else
                {
                    DM.SetLines(InitWindow);
                }
                break;
            case "Crowbar":
                if (PDSO.triggers.Contains("Goal Heard") && PDSO.triggers.Contains("Front Door"))
                {
                    GameObject.Find("Boards").GetComponent<Image>().enabled = false;
                    PDSO.triggers.Add("Boards Removed");

                    IM.HoldItem("");
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
