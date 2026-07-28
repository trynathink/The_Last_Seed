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

	// windmill outside
	[SerializeField] private ScriptsSO firstWindmillSight;
	[SerializeField] private ScriptsSO brokenPanel;
	[SerializeField] private Animator windmillAnim;
	const string seenWindmillTrigger = "WindmillSeen";
	const string panelFixedTrigger = "WindmillPanelFixed";
	// TODO: should probably have a table or enum for triggers, since these will be used elsewhere too

	// windmill inside
	[SerializeField] private ScriptsSO missingRod;
	[SerializeField] private ScriptsSO shovelAttempt;
	[SerializeField] private GameObject brick;
	private int brickStage = 0;
	const string handleFixedTrigger = "WindmillHandleFixed";
	const string brickTrigger = "BrickOut";
	const string shovelTrigger = "ShovelNeedsRope";

    // This script manages every scene in Act 2

    private void Start()
    {
        switch (SceneManager.GetActiveScene().name)
        {
			case "A2 Windmill Outside":
				if (!PDSO.triggers.Contains(seenWindmillTrigger))
				{
					DM.SetLines(firstWindmillSight);
					PDSO.triggers.Add(seenWindmillTrigger);
				}

				if (PDSO.triggers.Contains(panelFixedTrigger))
				{
					FixWindmill(string.Empty);
				}
				break;
			case "A2 Windmill Inside":
				if (PDSO.triggers.Contains(brickTrigger))
				{
					BrickOut();
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

	private void FixWindmill(string item)
	{
		if (!String.IsNullOrEmpty(item))
		{
			PDSO.RemoveItem(item);
			IM.HoldItem(string.Empty);
		}

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
                if (!PDSO.triggers.Contains(panelFixedTrigger))
				{
					FixWindmill(PDSO.HeldItem);
					PDSO.triggers.Add(panelFixedTrigger);
				}
				break;
			}
			default:
				DM.SetLines(DefaultItemFail);
				break;
		}
	}

	private void BrickOut()
	{
		brick.SetActive(false);
	}

	public void TapBrick()
	{
		switch (++brickStage)
		{
			case 1:
				break;
			case 2:
				break;
			case 3:
				if (brick.activeSelf)
				{
					BrickOut();
					PDSO.triggers.Add(brickTrigger);
				}
				break;
		}
	}

	public void MissingRod()
	{
		switch (PDSO.HeldItem)
		{
			case "":
				DM.SetLines(missingRod);
				break;
			case "Shovel Handle":
                if (!PDSO.triggers.Contains(shovelTrigger))
				{
					PDSO.triggers.Add(shovelTrigger);
				}
				DM.SetLines(shovelAttempt);
				break;
			case "Reinforced Handle":
                if (!PDSO.triggers.Contains(handleFixedTrigger))
				{
					PDSO.triggers.Add(handleFixedTrigger);
				}
				break;
			default:
				DM.SetLines(DefaultItemFail);
				break;
		}
	}
}
