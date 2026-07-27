using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Audio;

// Gaurav Singh

public class DialogueManager : MonoBehaviour
{
	[Header("Randomization Variables")]
	[SerializeField]
	private GameObject[] Bubbles;

	[SerializeField]
	private GameObject interactableBubble;

	[SerializeField]
	private float[] BubbleChances;

	// NOTE: Should list the audio generators in order by short, medium, long
	[SerializeField]
	private AudioResource[] BubbleAudio;

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

	[SerializeField]
	private int[] CharacterLimits;

	[SerializeField]
	private RectTransform TextArea;

	[Header("Core")]
    [SerializeField]
    bool Dia, Self;

    [SerializeField]
    ScriptsSO script;

    [SerializeField]
    int LineNum;

    [SerializeField]
    private UnityEvent onClick;

    [SerializeField]
    private InputActionReference click;

    [SerializeField]
    private AudioSource sound;

    Image DiaImg;
    GameObject Inner, NPC, Choice;
	private bool keepWord = false;

    void Start()
    {
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

		if (click != null)
		{
			click.action.performed += OnPointerClick;
			click.action.Enable();
		}
    }

    private void OnPointerClick(InputAction.CallbackContext context)
    {
        if (Dia)
        {
            if (++LineNum < script.Lines.Count)
            {
                if (Self)
                {
                    TalkingToMyself();
                }
                else
                {
                    NPCSpeak();
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

            NPCSpeak();
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
		float choice = Random.value;

		for (int i = 0; i < BubbleChances.Length; i++)
		{
			if (choice <= BubbleChances[i])
			{
				if (i != previous || Random.value < CurrentBubbleChance)
					return i;
			}
		}

		return 0;
	}

    // Remove wN
    // Use LineNum to switch to next line
    // Method that gets called to switch to next line
    void NPCSpeak()
    {
		NPCReset();

        string[] line = script.Lines[LineNum].Split(' ');
		const char interactable = '^';
		Vector2 placement = TextArea.rect.position;
		placement.y += TextArea.rect.height;
		float end = placement.x + TextArea.rect.width;

		int bubble = RandBubble(0);
		int limit = CharacterLimits[bubble];
		string words = line[0] + ' ';

		for (int i = 1; i < line.Length; i++)
		{
			string word = line[i];

			if (words.Length + word.Length > limit || word[0] == interactable || words[0] == interactable)
			{
				// TODO: prefab not always small for interactable word
				float bubbleWidth = NPCBubble(bubble, placement, words);
				float moveRightBuffer = Random.Range(MoveRightBufferRange.x, MoveRightBufferRange.y);
				placement.x += bubbleWidth + moveRightBuffer;
				// TODO: possibly change y position just a little bit each time

				if (Random.value <= MoveDownChance || placement.x > end)
				{
					placement.y -= Random.Range(MoveDownRange.x, MoveDownRange.y);
					float padding = Random.Range(LinePaddingRange.x, LinePaddingRange.y);
					placement.x = TextArea.rect.x + padding;
				}

				bubble = RandBubble(bubble);
				limit = CharacterLimits[bubble];
				words = "";
			}

			words += word + ' ';
		}

		// TODO: prefab not always right size for last word(s)
		NPCBubble(bubble, placement, words);
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

	private IEnumerator PlayAudio(AudioResource audio)
	{
		while (sound.isPlaying)
		{
			yield return new WaitForEndOfFrame();
		}

		sound.generator = (IAudioGenerator) audio;
		sound.Play();
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
			bubbleObject = Instantiate(Bubbles[bubble], NPC.transform);
		}
		
		if (sound != null) StartCoroutine(PlayAudio(BubbleAudio[bubble]));
		RectTransform transform = bubbleObject.GetComponent<RectTransform>();
		transform.localPosition = placement;
        bubbleObject.transform.GetChild(0).GetComponent<TMP_Text>().text = words;
		return transform.rect.width;
    }

    void NPCReset()
    {
		StopAllCoroutines();
		sound.Stop();

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
            GameObject.Find(script.itemGain.Name).GetComponent<Collectible>().Collect(script.itemGain);
        }

        if(script.trigger != string.Empty && script.trigger != null)
        {
            GameObject.Find("Canvas").GetComponent<GameSceneManager>().addTrigger(script.trigger);
        }

        if (script.SceneChange != string.Empty && script.SceneChange != null)
        {
            GameObject.Find("Canvas").GetComponent<GameSceneManager>().NextScene(script.SceneChange);
        }

        if (script.choice != null)
        {
			if (!keepWord)
			{
				script = script.choice.Outcomes[0];
			}

			SetChoice();
        }
        else
        {
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
            Transform c = Choice.transform.GetChild(i);

            c.gameObject.SetActive(true);
            
            c.GetComponent<Button>().enabled = true;
            c.GetComponent<Image>().enabled = true;
            c.GetChild(0).GetComponent<TMP_Text>().text = script.choice.Choices[i];
        }
    }

    public void Chose(int c)
    {
        SetLines(script.choice.Outcomes[c]);
    }

    void ResetChoice()
    {
        foreach(Transform c in Choice.transform)
        {
            c.gameObject.SetActive(false);
        }
    }
}
