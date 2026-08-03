using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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

	// Lion
	public ScriptsSO LionInit;
	public ScriptsSO LionIdle;
	public ScriptsSO LionConvinced;
	public ScriptsSO LionSack;
	public ScriptsSO LionLumber;
	public ScriptsSO LionCrop;

	// Crowd
	public ScriptsSO CrowdCropHint;
	public ScriptsSO CrowdLumberHint;
	public ScriptsSO CrowdSackHint;
	public ScriptsSO CrowdLionPraise;

	private ScriptsSO[] crowdLines;
	private int crowdLineIndex;

    // Bird
    [SerializeReference]
	ScriptsSO BirdTrustMinigame;

	// windmill outside
	[SerializeField] private ScriptsSO firstWindmillSight;
	[SerializeField] private ScriptsSO brokenPanel, fixedPanel;
	[SerializeField] private Animator windmillAnim;
	[SerializeField] private Image windmillPanels;
	[SerializeReference] private Sprite windmillFixed;
	const string seenWindmillTrigger = "WindmillSeen";
	const string panelFixedTrigger = "WindmillPanelFixed";
	// TODO: should probably have a table or enum for triggers, since these will be used elsewhere too

	// windmill inside
	[SerializeField] private ScriptsSO missingRod;
	[SerializeField] private ScriptsSO shovelAttempt, reinforcedShovelAttempt;
	[SerializeField] private GameObject brickOver, OpenBrick;
    [SerializeReference]
    GameObject ReinforcedPole;

    private int brickStage = 0;
	const string handleFixedTrigger = TriggerNames.WINDMILL_HANDLE_FIXED;
	const string brickTrigger = TriggerNames.BRICK;
	const string shovelTrigger = TriggerNames.SHOVEL;

    // This script manages every scene in Act 2

	private void Awake()
	{
		crowdLines = new ScriptsSO[4];
		crowdLines[0] = CrowdLionPraise;
		crowdLines[1] = CrowdCropHint;
		crowdLines[2] = CrowdSackHint;
		crowdLines[3] = CrowdLumberHint;
		crowdLineIndex = 0;
	}

    private void Start()
    {
        switch (SceneManager.GetActiveScene().name)
        {
			case SceneNames.ACT2_WINDMILL_OUTSIDE:
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
			case SceneNames.ACT2_WINDMILL_INSIDE:
				if (PDSO.triggers.Contains(brickTrigger))
				{
					BrickOut();
				}

				if (PDSO.triggers.Contains(handleFixedTrigger))
				{
					FixedPole();
                }
				break;
			case SceneNames.ACT2_HARE:
                if (PDSO.triggers.Contains(TriggerNames.HARE_NAKED_SCARECROW))
                {
                    NakedScarecrow();
                }
                break;
			case SceneNames.ACT2_BEAVER:
                if (PDSO.triggers.Contains(TriggerNames.WATERWHEEL_JAM_FIX))
                {
                    BlockageRemoval();
                }
				if (PDSO.triggers.Contains("Shovel Handle"))
				{
					GameObject scene = GameObject.Find("BG & Sprites");

					scene.transform.Find("Shovel Handle").gameObject.SetActive(false);
				}
                break;
			case SceneNames.ACT2_LION:
				if (!PDSO.triggers.Contains(TriggerNames.LION_SCENE_ENTRY))
				{
					DM.SetLines(LionInit);
					PDSO.triggers.Add(TriggerNames.LION_SCENE_ENTRY);
				}
				if (PDSO.triggers.Contains(TriggerNames.LION_QUESTION_ASKED)
					&& !PDSO.triggers.Contains(TriggerNames.LION_CONVINCED))
				{
					PDSO.triggers.RemoveAll(i => i == TriggerNames.LION_QUESTION_ASKED);
					PDSO.triggers.RemoveAll(i => i == TriggerNames.LION_CROP);
					PDSO.triggers.RemoveAll(i => i == TriggerNames.LION_SACK);
					PDSO.triggers.RemoveAll(i => i == TriggerNames.LION_LUMBER);
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

	[SerializeReference]
	Sprite nakedScarecrow;

	void NakedScarecrow()
	{
        GameObject.Find("Scarecrow").GetComponent<Image>().sprite = nakedScarecrow;
    }

    public void ScarecrowDialogue()
    {
        bool containsBlanketOrFabric = PDSO.Inventory
                        .Any(item => item.Name == CollectibleType.Blanket.ToString()
                                || item.Name == CollectibleType.Fabric.ToString());
        
        if(!containsBlanketOrFabric && PDSO.triggers.Contains(TriggerNames.WINDMILL_SCARECROW_CLOTH))
        {
            DM.SetLines(ScarecrowFabric);
        }
        else
        {
            DM.SetLines(ScarecrowIdle);
        }
    }

	public void LionDialogue()
	{
		Debug.Log("lion dialogue");
		if(PDSO.triggers.Contains(TriggerNames.LION_CONVINCED))
		{
			DM.SetLines(LionConvinced);
		}
		else if(PDSO.triggers.Contains(TriggerNames.LION_SACK) 
					&& PDSO.triggers.Contains(TriggerNames.LION_CROP)
					&& PDSO.triggers.Contains(TriggerNames.LION_LUMBER))
		{
			PDSO.triggers.Add(TriggerNames.LION_CONVINCED);
			DM.SetLines(LionConvinced);
		}
		else if(PDSO.triggers.Contains(TriggerNames.LION_QUESTION_ASKED))
		{
			switch (PDSO.HeldItem)
			{
				case ItemNames.LUMBER:
					DM.SetLines(LionLumber);
					break;
				case ItemNames.SACK:
					DM.SetLines(LionSack);
					break;
				case ItemNames.DEAD_CROP:
					DM.SetLines(LionCrop);
					break;
				default:
					DM.SetLines(LionIdle);
					break;
			}
		}
		else
		{
			DM.SetLines(LionIdle);
		}
	}

	public void CrowdDialogue()
	{
		if (!PDSO.triggers.Contains(TriggerNames.LION_CONVINCED))
		{
			DM.SetLines(crowdLines[crowdLineIndex]);
			crowdLineIndex = (crowdLineIndex + 1) % 4;
		}
	}

	[SerializeReference]
	ScriptsSO BeaverInit, BeaverIdleUnhappy, BeaverIdleHappy, BeaverIdleFixed, BeaverPanelMake;

	public void BeaverDia()
	{
		switch (PDSO.HeldItem)
		{
			case "":
				if (PDSO.triggers.Contains("BeaverInit"))
				{
					if (PDSO.triggers.Contains(TriggerNames.WATERWHEEL_BROK_FIX) && PDSO.triggers.Contains(TriggerNames.WATERWHEEL_JAM_FIX))
					{
						if (PDSO.triggers.Contains(panelFixedTrigger) && PDSO.triggers.Contains(handleFixedTrigger))
						{
							DM.SetLines(BeaverIdleFixed);
						}
						else
						{
                            DM.SetLines(BeaverIdleHappy);
                        }
					}
					else
					{
						DM.SetLines(BeaverIdleUnhappy);
					}
				}
				else
				{
					DM.SetLines(BeaverInit);
				}
				break;
			case ItemNames.LUMBER:
				if (!PDSO.triggers.Contains(TriggerNames.BEAVER_PANEL))
				{
                    DM.SetLines(BeaverPanelMake);
                }
                break;
			default:
                DM.SetLines(DefaultItemFail);
                break;
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
	
	// Windmill Outside Scene

	public void Sack(ItemSO sackI)
	{
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

		windmillPanels.sprite = windmillFixed;
	}

	public void WindmillPanels()
	{
		switch (PDSO.HeldItem)
		{
			case "":
			{

				if (!PDSO.triggers.Contains(panelFixedTrigger))
				{
                    const string trigger = TriggerNames.WINDMILL_SCARECROW_CLOTH;
                    if (!PDSO.ItemContains("Blanket") && !PDSO.triggers.Contains(trigger))
                    {
                        PDSO.triggers.Add(TriggerNames.WINDMILL_SCARECROW_CLOTH);
                    }
                    DM.SetLines(brokenPanel);
                }
				else
				{
						DM.SetLines(fixedPanel);
				}

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

	// Windmill Insider Scene

	private void BrickOut()
	{
		brickOver.SetActive(false);
		OpenBrick.SetActive(true);
	}

	[SerializeReference]
	Sprite b2, b3;

	public void TapBrick()
	{
		switch (++brickStage)
		{
			case 1:
				brickOver.GetComponent<Image>().sprite = b2;
				break;
			case 2:
				brickOver.GetComponent<Image>().sprite= b3;
				break;
			case 3:
				if (brickOver.activeSelf)
				{
					BrickOut();
					PDSO.triggers.Add(brickTrigger);
				}
				break;
		}
	}

	void FixedPole()
	{
		if (PDSO.triggers.Contains(handleFixedTrigger))
		{
			ReinforcedPole.SetActive(true);
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
			case "Reinforced Shovel":
                if (!PDSO.triggers.Contains(handleFixedTrigger))
				{
					PDSO.triggers.Add(handleFixedTrigger);
				}

				if (PDSO.ItemContains("Reinforced Shovel"))
				{
					PDSO.Inventory.Remove(PDSO.GetItem("Reinforced Shovel"));
				}
				FixedPole();
                DM.SetLines(reinforcedShovelAttempt);
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
				if (PDSO.triggers.Contains(TriggerNames.BLOCKAGE_ITEM))
				{
                    DM.SetLines(BlockageIdle); // does not add trigger
                }
				else
				{
                    DM.SetLines(IdleMetal); // adds "BlockageItem" trigger
                }
                break;
            case "Pitchfork":
                addTrigger(TriggerNames.WATERWHEEL_JAM_FIX);
                BlockageRemoval();

                if (!PDSO.triggers.Contains(TriggerNames.BLOCKAGE_ITEM))
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

				if (PDSO.triggers.Contains(TriggerNames.WATERWHEEL_JAM_FIX))
				{
					if (PDSO.triggers.Contains(TriggerNames.WATERWHEEL_BROK_FIX))
					{
                        DM.SetLines(WW);
                    }
					else
					{
                        DM.SetLines(WWBrok);
                    }
				}
				else if (PDSO.triggers.Contains(TriggerNames.WATERWHEEL_BROK_FIX))
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
			case "Paddle":
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
				if (PDSO.triggers.Contains(TriggerNames.ENGINE_OFF))
				{
					DM.SetLines(EngineOff);
				}
				else
				{
					DM.SetLines(EngineOn);
				}
				break;
			case "Crowbar":
                if (PDSO.triggers.Contains(TriggerNames.ENGINE_OFF))
                {
					if (PDSO.triggers.Contains(TriggerNames.SPADE_GAINED))
					{
						DM.SetLines(DefaultItemFail);
					}
					else
					{
						addTrigger(TriggerNames.SPADE_GAINED);
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
