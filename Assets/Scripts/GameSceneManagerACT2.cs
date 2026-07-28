using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.Linq;

public class GameSceneManagerACT2 : GameSceneManagerBase
{
    // Gaurav Singh

    // scarecrow
    public ScriptsSO ScarecrowIdle;
    public ScriptsSO ScarecrowFabric;

    // hare
    public ScriptsSO HareInit;
    public ScriptsSO HareIdle;

    // This script manages every scene in Act 2

    protected override void OnEnable()
    {
		base.OnEnable();

        switch (SceneManager.GetActiveScene().name)
        {
            
        }
    }

    public override void NextScene(string sceneName)
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

<<<<<<< HEAD
        IM.HoldItem("");

        PDSO.PlayerLocation = sceneName;
        SceneManager.LoadScene(PDSO.PlayerLocation);
=======
		base.NextScene(sceneName);
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
>>>>>>> dfcfb22c2762df5d0731fd20c7ab0301315f82d6
    }
}
