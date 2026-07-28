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

	// windmill
	[SerializeField] private ScriptsSO brokenPanel;
	[SerializeField] private Animator windmillAnim;
	const string windmillFixedTrigger = "WindmillFixed";

    // This script manages every scene in Act 2

    protected override void OnEnable()
    {
		base.OnEnable();

        switch (SceneManager.GetActiveScene().name)
        {
			case "A2 Windmill Outside":
				if (PDSO.triggers.Contains(windmillFixedTrigger))
				{
					FixWindmill();
				}
				break;
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
    }

	private void FixWindmill()
	{
		windmillAnim.SetTrigger("fix");
	}

	public void WindmillPanels()
	{
		switch (PDSO.HeldItem)
		{
			case "":
			{
				const string trigger = "WindmillScarecrowCloth";
				if (!PDSO.ItemContains("Blanket") && ! PDSO.triggers.Contains(trigger))
				{
					PDSO.triggers.Add("WindmillScarecrowCloth");
				}
				DM.SetLines(brokenPanel);
				break;
			}
			case "Blanket": case "Fabric":
			{
                if (!PDSO.triggers.Contains(windmillFixedTrigger))
				{
					FixWindmill();
					PDSO.triggers.Add(windmillFixedTrigger);
				}
				break;
			}
			default:
				DM.SetLines(DefaultItemFail);
				break;
		}
	}
}
