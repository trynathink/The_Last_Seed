using System.Collections.Generic;
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
    int LineNum;

    [SerializeField]
    Image DiaImg;

    [SerializeField]
    private UnityEvent onClick;

    [SerializeField]
    GameObject Inner;

    void Start()
    {
        DiaImg = GameObject.Find("Dialogue Image").GetComponent<Image>();

        Inner = transform.Find("Inner Text").gameObject;
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
        script = givenscript;
        Dia = true;
        DiaImg.enabled = true;
        //SceneChange = string.Empty;

        // Finding Which Textbox to use.
        if (givenscript.Character != string.Empty)
        {
            Self = false;

            // script for character dia
        }
        else
        {
            Self = true;

            Inner.GetComponent<Image>().enabled = true;

            Inner.GetComponent<Animator>().SetTrigger("Open");
            
            var t = Inner.transform.GetChild(0).GetComponent<TMP_Text>();
            t.enabled = true;
            t.text = givenscript.Lines[0];

            TalkingToMyself();
        }
    }

    void TalkingToMyself()
    {
        Debug.Log(Inner.transform.GetChild(0).GetComponent<TMP_Text>());

        var t = Inner.transform.GetChild(0).GetComponent<TMP_Text>();
        t.text = script.Lines[LineNum];
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
            // See if the player has a choice to make here
            /*if (choiceTrigger != null)
            {
                CS.TriggerChoice(choiceTrigger);
            }*/
        }

        
        Dia = false;
        DiaImg.enabled = false;
    }
}
