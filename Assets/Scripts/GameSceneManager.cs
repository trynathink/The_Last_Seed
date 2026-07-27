using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour
{
    // Gaurav Singh

    // This script manages every scene in Act 1 minus the main menu

    [SerializeField] 
    PlayerDataSO PDSO;

    DialogueManager DM;
    InventoryManager IM;

    [SerializeField] private AudioClip alarm;

    [SerializeReference]
    ScriptsSO bearIdle, bearInit;

    private bool isAlarmOff = false;

    // there is clearly a better way to do this, not rn
    public ScriptsSO AlarmOn, FrontDoorLock, FrontDoorGoal, InitWindow, leaveWindow, questionWindow, DefaultItemFail;

    private void OnEnable()
    {
        DM = GameObject.Find("Dialogue Manager").GetComponent<DialogueManager>();
        IM = GameObject.Find("Inventory").GetComponent<InventoryManager>();

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

	private void CatchAnyItemHeld(Action actionIfNoneHeld)
	{
		if (PDSO.HeldItem == "")
			actionIfNoneHeld();
		else
			DM.SetLines(DefaultItemFail);
	}

    // All scenes
    public void Item(ScriptsSO script)
    {
        IM.HoldItem("");
		CatchAnyItemHeld(() => DM.SetLines(script));
    }

    public void addTrigger(string t)
    {
        if (!PDSO.triggers.Contains(t))
        {
            PDSO.triggers.Add(t);
        }
    }

    public void NextScene(string sceneName)
    {
        PDSO.PlayerLocation = sceneName;
        SceneManager.LoadScene(PDSO.PlayerLocation);
    }

    // Bedroom & Bed Window Scene 
    public void OnClockClick()
    {
		Action click = () => {
			if (!isAlarmOff)
			{
				Clock();
				PDSO.triggers.Add("Clock");
			}
		};
		CatchAnyItemHeld(click);
    }

    public void Clock()
    {
        isAlarmOff = true;
		BackgroundSFX.StopLoop();
    }

    // Bedroom Scene
    public void BedroomWindow()
    {
		CatchAnyItemHeld(() => NextScene("A1 Bed Window"));
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
		Action click = () => {
			if (PDSO.triggers.Contains("Goal Heard"))
			{
				DM.SetLines(FrontDoorGoal);
			}
			else
			{
				DM.SetLines(FrontDoorLock);
			}
		};
		CatchAnyItemHeld(click);
    }

    public void Closet(bool open)
    {
		Action click = () => {
			GameObject.Find("Closet Closed").GetComponent<Image>().enabled = !open;
			GameObject.Find("Closet Open").GetComponent<Image>().enabled = open;
			GameObject.Find("Closet Open").transform.Find("Close Open Hitbox").gameObject.SetActive(open);
			MoveSFX.Play();
		};
		CatchAnyItemHeld(click);
    }

    public void BearDia()
    {
		Action click = () => {
			if(PDSO.triggers.Contains("Goal Heard"))
			{
				DM.SetLines(bearIdle);
			}
			else
			{
				DM.SetLines(bearInit);
			}
		};
		CatchAnyItemHeld(click);
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
                }
                else
                {
                    DM.SetLines(DefaultItemFail);
                }
                break;
            default:
                DM.SetLines(DefaultItemFail);
                break;
        }
    }

    

}
