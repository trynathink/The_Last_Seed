using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManagerACT2 : MonoBehaviour
{
    // Gaurav Singh

    // This script manages every scene in Act 1 minus the main menu

    [SerializeField]
    PlayerDataSO PDSO;

    DialogueManager DM;
    InventoryManager IM;
    MoveSFX MSFX;

    // there is clearly a better way to do this, not rn
    public ScriptsSO DefaultItemFail;

    private void OnEnable()
    {
        DM = GameObject.Find("Dialogue Manager").GetComponent<DialogueManager>();
        IM = GameObject.Find("Inventory").GetComponent<InventoryManager>();
        MSFX = GameObject.Find("SFX").GetComponent<MoveSFX>();

        switch (SceneManager.GetActiveScene().name)
        {
            
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
}
