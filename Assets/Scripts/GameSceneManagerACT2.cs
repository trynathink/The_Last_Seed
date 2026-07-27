using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.Linq;

public class GameSceneManagerACT2 : MonoBehaviour
{
    // Gaurav Singh

    // This script manages every scene in Act 2 minus the main menu

    [SerializeField]
    PlayerDataSO PDSO;

    DialogueManager DM;
    InventoryManager IM;
    MoveSFX MSFX;

    // scarecrow
    public ScriptsSO ScarecrowIdle;
    public ScriptsSO ScarecrowFabric;

    // hare
    public ScriptsSO HareInit;
    public ScriptsSO HareIdle;

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
        switch (PDSO.FireStage)
        {
            case 0:
                PDSO.Fire += 0.5f;
                break;
            case 1:
                PDSO.Fire += 1.5f;
                break;
            case 2:
                PDSO.Fire += 2.5f;
                break;
            case 3:
                break;
        }

        PDSO.PlayerLocation = sceneName;
        SceneManager.LoadScene(PDSO.PlayerLocation);
    }

    public void HareDialogue()
    {
        DM.SetLines(HareInit);
    }

    public void ScarecrowDialogue()
    {
        bool containsBlanketOrFabric = PDSO.Inventory
                        .Any(item => item.Name == CollectibleType.Blanket.ToString()
                                || item.Name == CollectibleType.Fabric.ToString());
        
        if(!containsBlanketOrFabric && PDSO.triggers.Contains("WindmillScarecrowCloth"))
        {
            DM.SetLines(ScarecrowFabric);
        }
        else
        {
            DM.SetLines(ScarecrowIdle);
        }
    }
}
