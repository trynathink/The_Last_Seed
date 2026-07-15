using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Gaurav Singh

public class DialogueManager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    bool Dia, Self;

    [SerializeField]
    ScriptsSO script;

    [SerializeField]
    int LineNum, WordNum;

    [SerializeField]
    Image DiaImg;

    [SerializeField]
    private UnityEvent onClick;

    [SerializeField]
    GameObject Inner, NPC, Choice;

    void Start()
    {
        DiaImg = GameObject.Find("Dialogue Image").GetComponent<Image>();

        Inner = transform.Find("Inner Text").gameObject;
        NPC = transform.Find("NPC Text").gameObject;
        Choice = transform.Find("Choice Text").gameObject;
    }

    // switch to IA
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (Dia)
        {
            LineNum++;

            if (!(LineNum >= script.Lines.Count))
            {
                if (Self)
                {
                    TalkingToMyself();
                }
                else
                {
                    NPCSpeak();

                    /*Debug.Log($"Line Num :{LineNum}, Word Count :{script.WordCount.Count}");

                    if (!(LineNum >= script.WordCount.Count))
                    {
                        NPCSpeak(WordNum);
                    }
                    else
                    {
                        StageLeft();
                    }*/
                }
            }
            else
            {
                StageLeft();
            }
        }
    }

    public void SetLines(ScriptsSO givenscript)
    {
        // Reseting Vars
        LineNum = 0;
        WordNum = 0;
        script = givenscript;
        DiaImg.enabled = true;
        ResetChoice();

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

    // Remove wN
    // Use LineNum to switch to next line
    // Method that gets called to switch to next line
    void NPCSpeak()
    {
        NPCReset();

        string line = script.Lines[LineNum];

        /*for (int i = wN; i < wN+script.WordCount[LineNum]; i++)
        {
            NPCWord(script.NPCplacement[i], script.Lines[i]);

            WordNum = i+1;
        }*/
    }

    // Vector 2 & string word
    // Method that places the strips of paper
    void NPCWord(int placement, string word)
    {
        var bubble = NPC.transform.GetChild(placement);

        bubble.gameObject.SetActive(true);

        bubble.GetComponent<Image>().enabled = true;
        bubble.GetChild(0).GetComponent<TMP_Text>().text = word;
    }

    // Reset
    void NPCReset()
    {
        foreach(Transform bubble in NPC.transform)
        {
            bubble.gameObject.SetActive(false);
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

        if (script.choice != null)
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
