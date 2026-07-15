using UnityEngine;
using UnityEngine.SceneManagement;

public class BedroomButtonManager : MonoBehaviour
{
    // Vinayak Karuppasamy

    // This class is responsible for handling all button interactions
    // in the bedroom scene

    [SerializeField] 
    PlayerDataSO PDSO;

    [SerializeReference]
    DialogueManager DM;

    [SerializeField] private AudioSource alarm;

    private bool isAlarmOff = false;



    public ScriptsSO AlarmOn, DefaultItemFail;

    private void OnEnable()
    {
        DM = GameObject.Find("Dialogue Manager").GetComponent<DialogueManager>();

        if (PDSO.triggers.Contains("Clock"))
        {
            Clock();
        }
    }

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

    // for any items without item specific interactions
	public void Item(ScriptsSO script)
    {
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


    public void ExitBedroom()
    {
        if(!isAlarmOff)
        {
            DM.SetLines(AlarmOn);

            return;
        }

        NextScene("A1 Living Room");
    }

    private void NextScene(string sceneName)
    {
        PDSO.PlayerLocation = sceneName;
        SceneManager.LoadScene(PDSO.PlayerLocation);
    }
}
