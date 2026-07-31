using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BedroomWindowButtonManager : MonoBehaviour
{
    // Vinayak Karuppasamy
    // Gaurav Singh (this may be a small ship of theseus moment)
    
    // This class manages button clicks for the window scene

    [SerializeField]
    AudioSource alarm;

    [SerializeField]
    PlayerDataSO PDSO;

    [SerializeReference]
    DialogueManager DM;

    public ScriptsSO DefaultItemFail;

    private void OnEnable()
    {
        DM = GameObject.Find("Dialogue Manager").GetComponent<DialogueManager>();

        if (PDSO.triggers.Contains("Clock"))
        {
            Clock();
        }
    }

    public void Clock()
    {
        alarm.Stop();
    }

    public void ExitWindow()
    {
        switch (PDSO.HeldItem)
        {
            case "":
                NextScene(SceneNames.ACT1_BEDROOM);
                break;
            default:
                DM.SetLines(DefaultItemFail);
                break;
        }
    }

    private void NextScene(string sceneName)
    {
        PDSO.PlayerLocation = sceneName;
        SceneManager.LoadScene(PDSO.PlayerLocation);
    }
}