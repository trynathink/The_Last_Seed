using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
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
	[SerializeField] private ScriptsSO hareIdleAdded;
	[SerializeField] private ScriptsSO[] hareAfterSubgoalOutcomes;
	[SerializeField] private ScriptsSO harePlantIdle;

	// Lion
	public ScriptsSO LionInit;
	public ScriptsSO LionIdle0;
    public ScriptsSO LionItemHint;
    public ScriptsSO LionIdle1;
    public ScriptsSO LionSack;
    public ScriptsSO LionLumber;
    public ScriptsSO LionCrop;
    public ScriptsSO LionIdle1a;
    public ScriptsSO LionIdle1b;
    public ScriptsSO LionIdle1c;
    public ScriptsSO LionIdle2;
    public ScriptsSO LionIdle3;
    public ScriptsSO LionIdle4;
    public ScriptsSO LionIdle5;
	public ScriptsSO LionWaterLie;
    public ScriptsSO LionIdle6;

	// Lion Scene - Fire
	public ScriptsSO FireInit;
	public ScriptsSO FireBeforeLionIdle3;

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
	[SerializeField] private ScriptsSO brokenPanel, fixedPanel, cantFixPanel;
	[SerializeField] private ScriptsSO brokenWindmill;
	[SerializeField] private ScriptsSO sackOpen;
	[SerializeField] private Animator windmillAnim;
	[SerializeField] private Image windmillPanels;
	[SerializeReference] private Sprite windmillFixed;

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
        Cursor.SetCursor(default, default, CursorMode.ForceSoftware);
    }

    private void Start()
    {
        switch (SceneManager.GetActiveScene().name)
        {
			case SceneNames.ACT2_WINDMILL_OUTSIDE:
				if (!PDSO.triggers.Contains(TriggerNames.WINDMILL_SEEN))
				{
					DM.SetLines(firstWindmillSight);
					PDSO.triggers.Add(TriggerNames.WINDMILL_SEEN);
				}

				if (PDSO.triggers.Contains(TriggerNames.WINDMILL_PANEL_FIXED))
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
					FixedPole(true);
                }
				else
				{
					FixedPole(false);
				}
				break;
			case SceneNames.ACT2_HARE:
                if (PDSO.triggers.Contains(TriggerNames.HARE_NAKED_SCARECROW))
                {
                    NakedScarecrow();
                }

				if (PDSO.triggers.Contains(TriggerNames.HARE_MORE_DIALOGUE))
				{
					HareSubgoals();
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
				if (PDSO.triggers.Contains(TriggerNames.SPADE_GAINED))
				{
					spadeImage.SetActive(false);
				}
                break;
			case SceneNames.ACT2_BIRD:
				BirdFace(false, "no");
				videoPanel.SetActive(false);
				PDSO.triggers.RemoveAll(i => i == TriggerNames.SEED_DIG);
				PDSO.triggers.RemoveAll(i => i == TriggerNames.SEED_WATER);
				PDSO.triggers.RemoveAll(i => i == TriggerNames.SEED_PLANT);
				break;
			case SceneNames.ACT2_LION:

                crowdLines = new ScriptsSO[4];
                crowdLines[0] = CrowdLionPraise;
                crowdLines[1] = CrowdCropHint;
                crowdLines[2] = CrowdSackHint;
                crowdLines[3] = CrowdLumberHint;
                crowdLineIndex = 0;

                if (!PDSO.triggers.Contains(TriggerNames.LION_SCENE_ENTRY))
				{
					DM.SetLines(LionInit);
				}
				break;
				
        }
    }

	private void HareSubgoals()
	{
		int subgoals = 0;
		if (PDSO.triggers.Contains(TriggerNames.LION_FIN)) subgoals++;
		if (PDSO.triggers.Contains(TriggerNames.SPADE_GAINED)) subgoals++;
		if (PDSO.triggers.Contains(TriggerNames.BIRD_CONVICED2)) subgoals++;
		hareIdleAdded.choice.Outcomes[hareIdleAdded.choice.Outcomes.Count-1] = hareAfterSubgoalOutcomes[subgoals];
		HareIdle = hareIdleAdded;
	}

    public override void NextScene(string sceneName)
    {
        switch (PDSO.FireStage)
        {
            case 0:
                PDSO.Fire = Mathf.Clamp(PDSO.Fire+0.5f, 0, 10);
                break;
            case 1:
                PDSO.Fire = Mathf.Clamp(PDSO.Fire + 1f, 0, 20);
                break;
            case 2:
                PDSO.Fire = Mathf.Clamp(PDSO.Fire + 1.5f, 0, 30);
                break;
            case 3:
                PDSO.Fire = Mathf.Clamp(PDSO.Fire + 2f, 0, 30);
                break;
			case 4:
				break;
        }

		base.NextScene(sceneName);
    }

    public void FireStateUp()
	{
		PDSO.FireStage++;
	}

    public void HareDialogue()
    {
		if (PDSO.triggers.Contains(TriggerNames.HARE_NEW_IDLE))
		{
			DM.SetLines(harePlantIdle);
		}
		else if (PDSO.triggers.Contains(TriggerNames.HARE_FIRST_INTERACTION))
		{
			DM.SetLines(HareIdle);
		}
		else
		{
			DM.SetLines(HareInit);
		}
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
		bool notFirst = PDSO.triggers.Contains(TriggerNames.SCARECROW_FIRST_INTERACTION);
        
        if(!containsBlanketOrFabric && notFirst)
        {
            DM.SetLines(ScarecrowFabric);
        }
        else
        {
            DM.SetLines(ScarecrowIdle);
			if (!notFirst) PDSO.triggers.Add(TriggerNames.SCARECROW_FIRST_INTERACTION);
        }
    }

	public override void addTrigger(string t)
	{
		if (t == TriggerNames.HARE_NAKED_SCARECROW)
		{
			NakedScarecrow();
		}
		else if (t == TriggerNames.HARE_MORE_DIALOGUE && !PDSO.triggers.Contains(TriggerNames.HARE_MORE_DIALOGUE))
		{
			HareSubgoals();
		}

		base.addTrigger(t);
	}

	public void LionDialogue()
	{
		// Idle 6
		if (PDSO.triggers.Contains(TriggerNames.LION_IDLE_6))
		{
			DM.SetLines(LionIdle6);

			FireStateUp();
		}
		// Idle 5
		else if(PDSO.triggers.Contains(TriggerNames.LION_IDLE_5))
		{
			DM.SetLines(LionIdle5);
		}
		// Idle 4
		else if(PDSO.triggers.Contains(TriggerNames.LION_IDLE_4))
		{
			DM.SetLines(LionIdle4);
		}
		// Idle 3
		else if(PDSO.triggers.Contains(TriggerNames.LION_IDLE_3))
		{
			DM.SetLines(LionIdle3);
		}
		// Idle 2
		else if(PDSO.triggers.Contains(TriggerNames.LION_IDLE_2))
		{
			DM.SetLines(LionIdle2);
		}
		// IDLE 1
		else if(PDSO.triggers.Contains(TriggerNames.LION_IDLE_1))
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
					switch (LionItems())
					{
						case 0:
                            DM.SetLines(LionIdle1);
                            break;
						case 1:
                            DM.SetLines(LionIdle1a);
                            break;
                        case 2:
                            DM.SetLines(LionIdle1b);
                            break;
                        case 3:
                            DM.SetLines(LionIdle1c);
                            break;
                    }
					
					break;
			}
		}
		// IDLE 0
		else
		{
            switch (PDSO.HeldItem)
            {
                case ItemNames.LUMBER:
                    DM.SetLines(LionItemHint);
                    break;
                case ItemNames.SACK:
                    DM.SetLines(LionItemHint);
                    break;
                case ItemNames.DEAD_CROP:
                    DM.SetLines(LionItemHint);
                    break;
                default:
                    DM.SetLines(LionIdle0);
                    break;
            }
        }
	}

	int LionItems()
	{
		int i = 0;

		if (PDSO.triggers.Contains(TriggerNames.LION_CROP)) i++;
		if (PDSO.triggers.Contains(TriggerNames.LION_LUMBER)) i++;
        if (PDSO.triggers.Contains(TriggerNames.LION_SACK)) i++;

		return i;
    }

	public void LionSceneFireDialogue()
	{
		switch (PDSO.HeldItem)
		{
			case ItemNames.METAL:
				if(PDSO.triggers.Contains(TriggerNames.LION_IDLE_5))
				{
					DM.SetLines(LionWaterLie);
				}
				else
				{
					DM.SetLines(FireBeforeLionIdle3);
				}
				break;
			default:
				DM.SetLines(FireInit);
				break;
		}
	}

	public void CrowdDialogue()
	{
		if (!PDSO.triggers.Contains(TriggerNames.LION_IDLE_2))
		{
			DM.SetLines(crowdLines[crowdLineIndex]);
			crowdLineIndex = (crowdLineIndex + 1) % 4;
		}
	}

	[SerializeReference]
	ScriptsSO BeaverInit, beaverIdle, beaverLumber;
	[SerializeField] ChoiceSO beaverIdleHare, beaverIdleBoth;
	[SerializeField] ScriptsSO beaverNoneFixed, beaverPartialFixed, beaverFixed;
	[SerializeField] ChoiceSO beaverHelped;
	// TODO: going back to idle choices from word interaction should be changed dynamically, but can stay for now

	public void BeaverDia()
	{
		if(PDSO.HeldItem == ItemNames.LUMBER)
		{
			DM.SetLines(beaverLumber);

			return;
		}


		if (PDSO.triggers.Contains("BeaverInit"))
		{
			if (HasAllTriggers("BeaverShovelAsked", "BeaverEngineAsked"))
			{
				int last = beaverIdleBoth.Outcomes.Count - 1;

				if (HasAllTriggers(TriggerNames.WATERWHEEL_BROK_FIX, TriggerNames.WATERWHEEL_JAM_FIX))
				{
					if (HasAllTriggers(TriggerNames.WINDMILL_PANEL_FIXED, handleFixedTrigger))
					{
						beaverIdleBoth.Outcomes[last] = beaverFixed;
					}
					else
					{
						if (PDSO.ItemContains("Rope"))
						{
							beaverPartialFixed.choice = beaverHelped;
						}

						beaverIdleBoth.Outcomes[last] = beaverPartialFixed;
					}
				}
				else
				{
					beaverIdleBoth.Outcomes[last] = beaverNoneFixed;
				}

				beaverIdle.choice = beaverIdleBoth;
			}
			else if (PDSO.triggers.Contains("HareBeaverTalk"))
			{
				beaverIdle.choice = beaverIdleHare;
			}

			DM.SetLines(beaverIdle);
		}
		else
		{
			DM.SetLines(BeaverInit);
		}
	}

	[SerializeReference]
	ScriptsSO BirdIntro, BirdToken, BirdIdle, BirdMoveFin;

    public void BirdDialogue()
    {
        switch (PDSO.HeldItem)
        {
            case "":
				if (PDSO.triggers.Contains(TriggerNames.BIRD_CONVICED) && PDSO.triggers.Contains(TriggerNames.BIRD_TOKEN))
				{
                    DM.SetLines(BirdIdle);
                }
				else
				{
                    DM.SetLines(BirdIntro);
                }
				break;
            case "Tree Token":
                if (PDSO.triggers.Contains(TriggerNames.BIRD_CONVICED))
                {
                    DM.SetLines(BirdToken);
					PDSO.RemoveItem("Tree Token");
                }
                break;
			case "Seed":
				DM.SetLines(BirdMoveFin);
				break;
            default:
                DM.SetLines(DefaultItemFail);
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
			DM.SetLines(sackOpen);
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
				if (!PDSO.triggers.Contains(TriggerNames.WINDMILL_PANEL_FIXED))
				{
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
                if (PDSO.triggers.Contains(TriggerNames.WINDMILL_HANDLE_FIXED))
				{
					DM.SetLines(cantFixPanel);
				}
				else if (!PDSO.triggers.Contains(TriggerNames.WINDMILL_PANEL_FIXED))
				{
					FixWindmillPanel(PDSO.HeldItem);
					PDSO.triggers.Add(TriggerNames.WINDMILL_PANEL_FIXED);
				}
				break;
			}
			default:
				DM.SetLines(DefaultItemFail);
				break;
		}
	}

	private bool HasAllTriggers(params string[] triggers)
	{
		return PDSO.triggers.Intersect(triggers).Count() == triggers.Length;
	}

	public bool WindmillFixed()
	{
		return HasAllTriggers(TriggerNames.WINDMILL_PANEL_FIXED, TriggerNames.WINDMILL_HANDLE_FIXED);
	}

	public void WindmillInteract()
	{
		Action action = () => {
			{
				if (!WindmillFixed())
					DM.SetLines(brokenWindmill);
			}
		};
		CatchAnyItemHeld(action);
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

	void FixedPole(bool state)
	{
		ReinforcedPole.SetActive(state);
	}

	[SerializeField]
	ItemSO ReShovel;

	public void MissingRod()
	{
		switch (PDSO.HeldItem)
		{
			case "":
                if (!PDSO.triggers.Contains(handleFixedTrigger))
                {
                    DM.SetLines(missingRod);
                }
				else
				{
                    PDSO.triggers.Remove(handleFixedTrigger);
                    PDSO.AddToInventory(ReShovel);
                    FixedPole(false);
                }
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
                    PDSO.RemoveItem("Reinforced Shovel");
                    FixedPole(true);
                    DM.SetLines(reinforcedShovelAttempt);
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
    [SerializeField] ItemSO Spade;
    [SerializeField] GameObject spadeImage;
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
						spadeImage.SetActive(false);
                        FireStateUp();
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

	[SerializeReference]
	GameObject closedFace, openFace, happyFace;
    public void BirdFace(bool talking, string context)
    {
		if(PDSO.triggers.Contains(TriggerNames.FINAL))
		{
			Debug.Log("Bird Left");
			
            closedFace.SetActive(false);
            openFace.SetActive(false);
            happyFace.SetActive(false);
			GameObject.Find("Bird Animated").SetActive(false);

			return;
        }

		if (PDSO.triggers.Contains(TriggerNames.BIRD_CONVICED2) || context == "Bird Token Interaction")
		{
			closedFace.SetActive(false);
			openFace.SetActive(false);
			happyFace.SetActive(true);

			return;
		}

		if (talking)
		{
            closedFace.SetActive(false);
            openFace.SetActive(true);
            happyFace.SetActive(false);
        }
		else
		{
            closedFace.SetActive(true);
            openFace.SetActive(false);
            happyFace.SetActive(false);
        }
    }

	[SerializeField] private ScriptsSO missingStepsMsg;
	[SerializeField] private ScriptsSO digSeed;
	[SerializeField] private ScriptsSO plantSeed;
	[SerializeField] private ScriptsSO waterSeed;

	[SerializeField] private GameObject videoPanel;
	[SerializeField] private VideoPlayer videoPlayer;
	[SerializeField] private AudioSource audioSource;

	public void PlantSeed()
	{
		if (!PDSO.triggers.Contains(TriggerNames.BIRD_MOVE))
		{
			Debug.Log("Bird has not moved");
            return;
			
		}

		switch (PDSO.HeldItem) 
		{
			case ItemNames.SPADE:
				Debug.Log("Spade Interaction Happened");
				PDSO.triggers.Add(TriggerNames.SEED_DIG);
				DM.SetLines(digSeed);
				break;

			case ItemNames.SEED:
				if (!PDSO.triggers.Contains(TriggerNames.SEED_DIG))
				{
					DM.SetLines(missingStepsMsg);
				}
				else
				{
					PDSO.triggers.Add(TriggerNames.SEED_PLANT);
					DM.SetLines(plantSeed);
				}
				break;

			case ItemNames.WATER:
				if (!PDSO.triggers.Contains(TriggerNames.SEED_PLANT))
				{
					DM.SetLines(missingStepsMsg);
				}
				else
				{
					PDSO.triggers.Add(TriggerNames.SEED_WATER);
					DM.SetLines(waterSeed);

					BackgroundMusic.Stop();
					videoPanel.SetActive(true);
					videoPlayer.Play();
					audioSource.Play();
				}
				break;

			default:
				DM.SetLines(missingStepsMsg);
				break;
		}
	}
}
