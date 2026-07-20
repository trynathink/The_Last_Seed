using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

		click.action.performed += OnPointerClick;
		click.action.Enable();
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
			else if (!keepWord)
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
        Debug.Log(Inner.transform.GetChild(0).GetComponent<TMP_Text>());

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
		Vector2 placement = TextArea.rect.position;
		placement.y += TextArea.rect.height;
		float end = placement.x + TextArea.rect.width;

		int bubble = RandBubble(0);
		GameObject bubblePrefab = Bubbles[bubble];
		int limit = CharacterLimits[bubble];
		string words = "";

		for (int i = 0; i < line.Length; i++)
		{
			string word = line[i];

			if ((words.Length > 0 && words.Length + word.Length > limit) || i == line.Length - 1)
			{
				if (i == line.Length - 1)
				{
					bubblePrefab = Bubbles[2];
					words += word;
				}

				NPCBubble(bubblePrefab, placement, words);
				float moveRightBuffer = Random.Range(MoveRightBufferRange.x, MoveRightBufferRange.y);
				placement.x += bubblePrefab.GetComponent<RectTransform>().rect.width + moveRightBuffer;
				// TODO: possibly change y position just a little bit each time

				if (Random.value <= MoveDownChance || placement.x > end)
				{
					placement.y -= Random.Range(MoveDownRange.x, MoveDownRange.y);
					float padding = Random.Range(LinePaddingRange.x, LinePaddingRange.y);
					placement.x = TextArea.rect.x + padding;
				}

				bubble = RandBubble(bubble);
				bubblePrefab = Bubbles[bubble];
				limit = CharacterLimits[bubble];
				words = "";
			}

			words += word + ' ';
			// TODO: try to handle last words differently; some not being shown
		}
    }

	private void OnWordClick()
	{
		SetLines(script.choice.Outcomes[0]);
		Invoke("OnWordHoverExit", 0.25f);
	}

	private void OnWordHover()
	{
		keepWord = true;
	}

	private void OnWordHoverExit()
	{
		keepWord = false;
	}

    // Method that places the strips of paper
    void NPCBubble(GameObject bubblePrefab, Vector2 placement, string words)
    {
		GameObject bubble;
		int clickSymIdx = words.IndexOf('^');

		if (clickSymIdx >= 0)
		{
			words = words.Remove(clickSymIdx, 1);
			bubble = Instantiate(interactableBubble, NPC.transform);
			bubble.AddComponent<Button>().onClick.AddListener(OnWordClick);
			EventTrigger trigger = bubble.AddComponent<EventTrigger>();

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
			bubble = Instantiate(bubblePrefab, NPC.transform);
		}
		
		RectTransform transform = bubble.GetComponent<RectTransform>();
		transform.localPosition = placement;
        bubble.transform.GetChild(0).GetComponent<TMP_Text>().text = words;
    }

    void NPCReset()
    {
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

        if (script.itemGain != string.Empty && script.itemGain != null)
        {
            GameObject.Find(script.itemGain).GetComponent<Collectible>().Collect();
        }

        if(script.trigger != string.Empty && script.trigger != null)
        {
            GameObject.Find("Canvas").GetComponent<GameSceneManager>().addTrigger(script.trigger);
        }

        if (script.SceneChange != string.Empty && script.SceneChange != null)
        {
            GameObject.Find("Canvas").GetComponent<GameSceneManager>().NextScene(script.SceneChange);
        }

        if (script.choice != null && script.choice.Choices.Count > 0)
        {
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
