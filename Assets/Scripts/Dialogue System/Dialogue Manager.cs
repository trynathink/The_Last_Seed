using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;
using System.Reflection;
using Unity.VisualScripting;

// Gaurav Singh

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    PlayerDataSO PDSO;

    [SerializeField]
    InputAction ClickIA;

	[Header("Randomization Variables")]
	[SerializeField]
	private RandomContainer[] Bubbles;

	[SerializeField]
	private GameObject interactableBubble;

	[SerializeField]
	private float[] BubbleChances;

	[SerializeField]
	private AudioResource BubbleAudio;

	[SerializeField]
	private float CurrentBubbleChance;

	[SerializeField][Range(0, 1)]
	private float MoveDownChance;

	[SerializeField]
	private Vector2 MoveDownRange;

	[SerializeField]
	private Vector2 MoveRightBufferRange;

	[SerializeField]
	private Vector2 LinePaddingRange;
	private int[] CharacterLimits;

	[SerializeField]
	private RectTransform TextArea;

	[Header("Core")]
    [SerializeField]
    bool Dia;

    [SerializeField]
    bool Self;

    [SerializeField]
    ScriptsSO script;

    [SerializeField]
    int LineNum;

    [SerializeField]
    private UnityEvent onClick;

    [SerializeField]
    private AudioSource sound;

    Image DiaImg;
    GameObject Inner, NPC, Choice;
	private bool keepWord = false;
	private bool animStarted = false;
	private bool skipAnim = false;

    void Awake()
    {
        var playerMap = InputSystem.actions.FindActionMap("Player");
        playerMap.Enable();
        ClickIA = InputSystem.actions.FindAction("Attack");
        ClickIA.Enable();

        DiaImg = GameObject.Find("Dialogue Image").GetComponent<Image>();
        Inner = transform.Find("Inner Text").gameObject;
        NPC = transform.Find("NPC Text").gameObject;
        Choice = transform.Find("Choice Text").gameObject;

		// Modify bubble chances to work as thresholds when using a random number > 0 < 1
		float current = BubbleChances[0];
		for (int i = 1; i < BubbleChances.Length; i++)
		{
			current += BubbleChances[i];
			BubbleChances[i] = current;
		}

		CharacterLimits = new int[Bubbles.Length];
		for (int i = 0; i < Bubbles.Length; i++)
		{
			CharacterLimits[i] = Bubbles[i].Get().GetComponentInChildren<TMP_Text>().text.Length;
		}
    }

    void FixedUpdate()
    {
        if (Dia && ClickIA.phase == InputActionPhase.Started)
		//if (Dia && !sound.isPlaying)
        {
			if (animStarted)
			{
				skipAnim = true;
				animStarted = false;
				sound.Stop();
				PlayBlip();
			}
			else if (++LineNum < script.Lines.Count)
            {
                if (Self)
                {
                    TalkingToMyself();
                }
				else if (!keepWord)
                {
					NPCReset();
					StartCoroutine(NPCSpeak());
                }
            }
			else if (!keepWord && LineNum == script.Lines.Count)
            {
                StageLeft();
            }
        }
    }

    public void SetLines(ScriptsSO givenscript)
    {
        // Reseting Vars
        LineNum = 0;
        script = givenscript;
        DiaImg.enabled = true;
        ResetChoice();

        GameObject.Find("Inventory").GetComponent<InventoryManager>().HoldItem("");

        // Finding Which Textbox to use.
        if (givenscript.Character != string.Empty)
        {
            Self = false;
            Dia = true;

            if(givenscript.Character == "Bird")
            {
                GameObject.Find("Canvas").GetComponent<GameSceneManagerACT2>().BirdFace(true, givenscript.name);
            }

			NPCReset();
            StartCoroutine(NPCSpeak());
        }
        else
        {
            Self = true;

            Inner.GetComponent<Image>().enabled = true;
            
            var anim = Inner.GetComponent<Animator>();
            anim.SetTrigger("Open");
            StartCoroutine(InnerMonoAnimWait(anim.GetCurrentAnimatorStateInfo(0).length));
        }
    }

    IEnumerator InnerMonoAnimWait(float time)
    {
        yield return new WaitForSeconds(time);

        Dia = true;

        var t = Inner.transform.GetChild(0).GetComponent<TMP_Text>();
        t.enabled = true;
        t.text = script.Lines[0];
    }

    void TalkingToMyself()
    {
        var t = Inner.transform.GetChild(0).GetComponent<TMP_Text>();
        t.text = script.Lines[LineNum];
    }


	int RandBubble(int previous)
	{
		float choice = UnityEngine.Random.value;

		for (int i = 0; i < BubbleChances.Length; i++)
		{
			if (choice <= BubbleChances[i])
			{
				if (i != previous || UnityEngine.Random.value < CurrentBubbleChance)
					return i;
			}
		}

		return 0;
	}

    // Remove wN
    // Use LineNum to switch to next line
    // Method that gets called to switch to next line
    private IEnumerator NPCSpeak()
    {
		animStarted = true;
        string[] line = script.Lines[LineNum].Split(' ');
		int wi = 0;
		int previous = -1;
		const char interactable = '^';
		Vector2 placement = TextArea.rect.position;
		placement.y += TextArea.rect.height;
		float end = placement.x + TextArea.rect.width;

		while(wi < line.Length)
		{
			int bubble = RandBubble(previous);
			int limit = CharacterLimits[bubble];
			string words = string.Empty;
			string word = line[wi];

			while (words.Length + word.Length < limit && wi < line.Length)
			{
				words += word + ' ';
				if (word[0] == interactable) break;
				word = line[++wi % line.Length];
			}

			if (words.Length == 0) continue;
			float bubbleWidth = NPCBubble(bubble, placement, words);
			previous = bubble;
			yield return new WaitWhile(() => sound.isPlaying && !skipAnim);

			float moveRightBuffer = UnityEngine.Random.Range(MoveRightBufferRange.x, MoveRightBufferRange.y);
			placement.x += bubbleWidth + moveRightBuffer;
			// TODO: possibly change y position just a little bit each time

			if (UnityEngine.Random.value <= MoveDownChance || placement.x > end)
			{
				placement.y -= UnityEngine.Random.Range(MoveDownRange.x, MoveDownRange.y);
				float padding = UnityEngine.Random.Range(LinePaddingRange.x, LinePaddingRange.y);
				placement.x = TextArea.rect.x + padding;
			}
		}

		animStarted = false;
    }

	private void OnWordClick()
	{
		NPCReset();
		keepWord = false;
		script = script.choice.Outcomes[1];
		SetChoice();
	}

	private void OnWordHover()
	{
		keepWord = true;
	}

	private void OnWordHoverExit()
	{
		keepWord = false;
	}

	private void PlayBlip()
	{
		if (sound != null) 
		{
			sound.generator = (IAudioGenerator) BubbleAudio;
			sound.Play();
		}
	}

    // Method that places the strips of paper; returns paper object width for convenience
    float NPCBubble(int bubble, Vector2 placement, string words)
    {
		GameObject bubbleObject;
		int clickSymIdx = words.IndexOf('^');

		if (clickSymIdx >= 0)
		{
			words = words.Remove(clickSymIdx, 1);
			bubbleObject = Instantiate(interactableBubble, NPC.transform);
			bubbleObject.AddComponent<Button>().onClick.AddListener(OnWordClick);
			EventTrigger trigger = bubbleObject.AddComponent<EventTrigger>();

			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerEnter;
			entry.callback.AddListener((eventData) => { OnWordHover(); });
			trigger.triggers.Add(entry);

			entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerExit;
			entry.callback.AddListener((eventData) => { OnWordHoverExit(); });
			trigger.triggers.Add(entry);
		}
		else
		{
			bubbleObject = Instantiate(Bubbles[bubble].Get(), NPC.transform);
		}
		
		if (!skipAnim) PlayBlip();	
		RectTransform transform = bubbleObject.GetComponent<RectTransform>();
		transform.localPosition = placement;
        bubbleObject.transform.GetChild(0).GetComponent<TMP_Text>().text = words;
		return transform.rect.width;
    }

    void NPCReset()
    {
		StopAllCoroutines();
		sound.Stop();
		skipAnim = false;
		animStarted = false;

        foreach(Transform bubble in NPC.transform)
        {
			Destroy(bubble.gameObject);
        }
    }

    void StageLeft()
    {
        if (Self)
        {
            Inner.GetComponent<Image>().enabled = false;

            var t = Inner.transform.GetChild(0).GetComponent<TMP_Text>();
            t.enabled = false;
        }
        else
        {
            NPCReset();
        }

        if (script.itemGain != null)
        {
            if (GameObject.Find(script.itemGain.Name) != null)
            {
                GameObject.Find(script.itemGain.Name).GetComponent<Collectible>().Collect(script.itemGain);
            }
            else
            {
                PDSO.AddToInventory(script.itemGain);
            }
            
        }

        if (script.trust != 0)
        {
            switch (script.Character)
            {
                case "Bird":
                    string check = script.trustVal;

                    if (!PDSO.triggers.Contains(check))
                    {
                        GameObject.Find("Canvas").SendMessage("addTrigger", check);

                        if(script.trust > 100)
                        {
                            GameObject.Find("Canvas").SendMessage("FireStateUp");
                        }
                        else
                        {
                            PDSO.BirdTrust += script.trust;
                        }
                    }
                    break;
            }
        }

        if (script.trigger != string.Empty && script.trigger != null)
        {
            GameObject.Find("Canvas").SendMessage("addTrigger", script.trigger);

            if(script.trigger == "MidPointFire")
            {
                PDSO.FireStage = 4;
            }
        }

        if (script.SceneChange != string.Empty && script.SceneChange != null)
        {
            GameObject.Find("Canvas").GetComponent<GameSceneManager>().NextScene(script.SceneChange);
        }

        if (script.choice != null)
        {
			if (!keepWord && script.choice.Choices.Count == 0)
			{
				script = script.choice.Outcomes[0];
			}

			SetChoice();
        }
        else
        {
            if(script.Character == "Bird")
            {
                GameObject.Find("Canvas").GetComponent<GameSceneManagerACT2>().BirdFace(false, "no");
            }

            Dia = false;
            DiaImg.enabled = false;
            LineNum = 0;
            script = null;
        }
    }

    void SetChoice()
    {
        for (int i = 0; i < script.choice.Choices.Count; i++)
        {
            if (script.Character == "Bird")
            {
                if (script.choice.TrigReq[i] == null || script.choice.TrigReq[i] == "" || PDSO.triggers.Contains(script.choice.TrigReq[i]))
                {
                    Transform c = Choice.transform.GetChild(i);
                    c.gameObject.SetActive(true);
                    c.GetComponent<Button>().enabled = true;
                    c.GetComponent<Image>().enabled = true;
                    c.GetChild(0).GetComponent<TMP_Text>().text = script.choice.Choices[i];
                }
            }
            else
            {
                Transform c = Choice.transform.GetChild(i);
                c.gameObject.SetActive(true);
                c.GetComponent<Button>().enabled = true;
                c.GetComponent<Image>().enabled = true;
                c.GetChild(0).GetComponent<TMP_Text>().text = script.choice.Choices[i];
            }
        }
    }

    [SerializeReference]
    ScriptsSO BirdFail;

    public void Chose(int c)
    {
        if (script.Character == "Bird")
        {
            if (script.choice.TrustReq[c] <= PDSO.BirdTrust)
            {
                SetLines(script.choice.Outcomes[c]);
            }
            else
            {
                if (script.choice.TrustFail.Count > 0)
                {
                    if (script.choice.TrustFail[c] != null)
                    {
                        SetLines(script.choice.TrustFail[c]);
                    }
                }
                else
                {
                    SetLines(BirdFail);
                }
            }
        }
        else
        {
            SetLines(script.choice.Outcomes[c]);
        }
    }

    void ResetChoice()
    {
        foreach(Transform c in Choice.transform)
        {
            c.gameObject.SetActive(false);
        }
    }
}
