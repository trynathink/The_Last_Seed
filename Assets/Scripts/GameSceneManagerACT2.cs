using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class GameSceneManagerACT2 : GameSceneManagerBase
{
    // Gaurav Singh

    // scarecrow
    public ScriptsSO ScarecrowIdle;
    public ScriptsSO ScarecrowFabric;

    // hare
    public ScriptsSO HareInit;
    public ScriptsSO HareIdle;

    // Bird
    [SerializeReference]
	ScriptsSO BirdTrustMinigame;

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
					FixWindmillPanel(string.Empty);
				}

				if (PDSO.triggers.Contains(handleFixedTrigger))
				{
					windmillAnim.SetBool("spin", true);
				}
				break;
			case "A2 Windmill Inside":
				if (PDSO.triggers.Contains(brickTrigger))
				{
					BrickOut();
				}
				break;
			case "A2 Beaver's River":
                if (PDSO.triggers.Contains("Blockage"))
                {
                    BlockageRemoval();
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

    public void BirdDialogue()
    {
        switch (PDSO.HeldItem)
        {
            case "":
                if (PDSO.triggers.Contains("BirdTrust"))
                {

                }
                else
                {
                    DM.SetLines(BirdTrustMinigame);
                }
                    break;
            case "Tree Token":
                if (PDSO.triggers.Contains("BirdTrust"))
                {

                }
                break;
        }
    }
	
	public void Sack(ItemSO sackI)
	{
		Debug.Log("ah");

		Image sack1 = GameObject.Find("Sack 1").GetComponent<Image>();
        Image sack2 = GameObject.Find("Sack 2").GetComponent<Image>();

        if (sack1.enabled)
		{
			sack1.enabled = false;
			sack2.enabled = true;
		}
		else if (sack2.enabled)
		{
            sack2.enabled = false;
            PDSO.Inventory.Add(sackI);
		}
	}

	private void FixWindmillPanel(string item)
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
					FixWindmillPanel(PDSO.HeldItem);
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

	// Beaver Scene

    [SerializeReference]
	ScriptsSO BlockageIdle, IdleMetal, PithforkMetal;

	public void Blockage()
	{
        switch (PDSO.HeldItem)
        {
            case "":
				if (PDSO.triggers.Contains("BlockageItem"))
				{
                    DM.SetLines(BlockageIdle); // does not add trigger
                }
				else
				{
                    DM.SetLines(IdleMetal); // adds "BlockageItem" trigger
                }
                break;
            case "Pitchfork":
                addTrigger("WWJamFix");
                BlockageRemoval();

                if (!PDSO.triggers.Contains("BlockageItem"))
                {
                    DM.SetLines(PithforkMetal); // adds "WWJamFix" + "BlockageItem" trigger
                }
                break;
            default:
                DM.SetLines(DefaultItemFail);
                break;
        }

        IM.HoldItem("");
    }

	void BlockageRemoval()
	{
		GameObject.Find("Blockage").SetActive(false);
	}

    [SerializeReference]
    ScriptsSO WWJamNBrok, WWJam, WWBrok, WW, LumberCraftHint, BrokFix;

    public void WaterWheel()
	{
		switch (PDSO.HeldItem)
		{
			case "":

				if (PDSO.triggers.Contains("WWJamFix"))
				{
					if (PDSO.triggers.Contains("WWBrokFix"))
					{
                        DM.SetLines(WW);
                    }
					else
					{
                        DM.SetLines(WWBrok);
                    }
				}
				else if (PDSO.triggers.Contains("WWBrokFix"))
				{
                    DM.SetLines(WWJam);
                }
				else
				{
                    DM.SetLines(WWJamNBrok);
                }

				break;
			case "Lumber":
				DM.SetLines(LumberCraftHint);
				break;
			case "Paddles":
				DM.SetLines(BrokFix);
				break;
			default:
				DM.SetLines(DefaultItemFail);
				break;
		}
	}

    [SerializeReference]
    ScriptsSO EngineOn, EngineOff, EngineOnCrowbar;
	ItemSO Spade;

    public void Engine()
	{
		switch (PDSO.HeldItem)
		{
			case "":
				if (PDSO.triggers.Contains("EngineOff"))
				{
					DM.SetLines(EngineOff);
				}
				else
				{
					DM.SetLines(EngineOn);
				}
				break;
			case "Crowbar":
                if (PDSO.triggers.Contains("EngineOff"))
                {
					if (PDSO.triggers.Contains("Spade Gained"))
					{
						DM.SetLines(DefaultItemFail);
					}
					else
					{
						addTrigger("Spade Gained");
						PDSO.Inventory.Add(Spade);
                    }
                }
                else
                {
                    DM.SetLines(EngineOnCrowbar);
                }
                break;
			default:
                DM.SetLines(DefaultItemFail);
                break;
		}
	}
}
