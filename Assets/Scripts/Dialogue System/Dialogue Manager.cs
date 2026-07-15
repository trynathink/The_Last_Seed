using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
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
                    Debug.Log($"Line Num :{LineNum}, Word Count :{script.WordCount.Count}");

                    if (!(LineNum >= script.WordCount.Count))
                    {
                        NPCSpeak(WordNum);
                    }
                    else
                    {
                        StageLeft();
                    }
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
        //SceneChange = string.Empty;

        // Finding Which Textbox to use.
        if (givenscript.Character != string.Empty)
        {
            Self = false;

            Dia = true;

            NPCSpeak(0);
        }
        else
        {
            Self = true;

            Inner.GetComponent<Image>().enabled = true;
            
            var anim = Inner.GetComponent<Animator>();
            anim.SetTrigger("Open");
            StartCoroutine(AnimWait(anim.GetCurrentAnimatorStateInfo(0).length));
        }
    }

    IEnumerator AnimWait(float time)
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

    void NPCSpeak(int wN)
    {
        NPCReset();

        for (int i = wN; i < wN+script.WordCount[LineNum]; i++)
        {
            NPCWord(script.NPCplacement[i], script.Lines[i]);

            WordNum = i+1;
        }
    }

    void NPCWord(int placement, string word)
    {
        var bubble = NPC.transform.GetChild(placement);
        
        bubble.GetComponent<Image>().enabled = true;
        bubble.GetChild(0).GetComponent<TMP_Text>().text = word;
    }

    void NPCReset()
    {
        foreach(Transform bubble in NPC.transform)
        {
            bubble.GetComponent<Image>().enabled = false;
            bubble.GetChild(0).GetComponent<TMP_Text>().text = "";
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

            c.GetComponent<Button>().interactable = true;
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
            c.GetComponent<Button>().interactable = false;
            c.GetChild(0).GetComponent<TMP_Text>().text = "";
        }
    }
}
