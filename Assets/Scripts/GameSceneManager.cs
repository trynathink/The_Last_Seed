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
    MoveSFX MSFX;

    [SerializeField] private AudioSource alarm;

    [SerializeReference]
    ScriptsSO bearIdle, bearInit;

    private bool isAlarmOff = false;

    // there is clearly a better way to do this, not rn
    public ScriptsSO AlarmOn, FrontDoorLock, FrontDoorGoal, InitWindow, leaveWindow, questionWindow, DefaultItemFail;



    private void OnEnable()
    {
        DM = GameObject.Find("Dialogue Manager").GetComponent<DialogueManager>();
        IM = GameObject.Find("Inventory").GetComponent<InventoryManager>();
        MSFX = GameObject.Find("SFX").GetComponent<MoveSFX>();

        switch (SceneManager.GetActiveScene().name)
        {
            case "A1 Bedroom":
                if (PDSO.triggers.Contains("Clock"))
                {
                    Clock();
                }
                break;
            case "A1 Bed Window":
                if (PDSO.triggers.Contains("Clock"))
                {
                    Clock();
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

    // All scenes
    public void Item(ScriptsSO script)
    {
        IM.HoldItem("");

        // for any items without item specific interactions
        switch (PDSO.HeldItem)
        {
            case "":
                DM.SetLines(script);
                break;
            default:
                DM.SetLines(DefaultItemFail);
                break;
        }
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
        alarm.Stop();
    }

    // Bedroom Scene
    public void BedroomWindow()
    {
        switch (PDSO.HeldItem)
        {
            case "":
                NextScene("A1 Bed Window");
                break;
            default:
                DM.SetLines(DefaultItemFail);
                break;
        }
    }

    public void ExitBedroom()
    {
        if (!isAlarmOff)
        {
            DM.SetLines(AlarmOn);

            return;
        }

        MSFX.door = true;
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

                MSFX.Play();
                break;
            default:
                DM.SetLines(DefaultItemFail);
                break;
        }
    }

    public void BearDia()
    {
        Debug.Log(PDSO.triggers);

        switch (PDSO.HeldItem)
        {
            case "":
                

                if(PDSO.triggers.Contains("Goal Heard"))
                {
                    Debug.Log("Idle");

                    DM.SetLines(bearIdle);
                }
                else
                {
                    Debug.Log("Init");

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
        MSFX.door = true;
        NextScene("A1 Bedroom");
    }

    public void LRtoK()
    {
        Debug.Log("move kitchen");

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
