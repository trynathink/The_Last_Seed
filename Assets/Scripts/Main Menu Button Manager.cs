using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtonManager : MonoBehaviour
{
    // Gaurav Singh

    // Contains the buttons methods for the Main Menu
    // Acts as the Manager for the Main Menu Screen

    public PlayerDataSO PDSO;

    GameObject TitleButtons, SaveButtons;

    //The code for how the save images change is sloppy code on my end (GS) but for now it will work
    // Reminder to return here and find a better way to do this
    [SerializeReference]
    List<Sprite> SImg, NImg;

    [SerializeReference]
    Sprite defaultCursor;

    [SerializeReference]
    List<GameObject> Saves;

    void Start()
    {
        TitleButtons = GameObject.Find("Title Screen");
        SaveButtons = GameObject.Find("Saves Screens");

        TitleButtons.SetActive(false);
        SaveButtons.SetActive(true);

        SaveImgSet();
    }

    // Does not appear in V.S.
    public void QuitButton()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    // Does not appear in V.S.
    public void PlayButton()
    {
        TitleButtons.SetActive(false);
        SaveButtons.SetActive(true);
    }

    // Does not appear in V.S.
    public void PlayScreenBackButton()
    {
        TitleButtons.SetActive(true);
        SaveButtons.SetActive(false);
    }

    public void PlayScreenSaveFileButton(string SaveName)
    {
        if (PDSO.CheckSave(SaveName))
        {
            PDSO.LoadSave(SaveName);
            SceneManager.LoadScene(PDSO.PlayerLocation);
        }
        else
        {
            PDSO.NewGame(SaveName);
            SceneManager.LoadScene(PDSO.PlayerLocation);
        }
    }

    public void PlayScreenDeleteSaveButton(string SaveName)
    {
        if (PDSO.CheckSave(SaveName))
        {
            PDSO.DeleteSave(SaveName);
        }
        else
        {
            Debug.Log("No save to Delete");
        }

        SaveImgSet();
    }

    void SaveImgSet()
    {
        foreach(GameObject s in Saves)
        {
            Debug.Log(s.name);

            int i = 0;
            if (PDSO.CheckSave(s.name))
            {
                s.GetComponent<Image>().sprite = SImg[i];
            }
            else
            {

                s.GetComponent<Image>().sprite = NImg[i];
                var sD = s.transform.GetChild(0).GetComponent<Image>();

                sD.sprite = null;
                sD.color = new Color(0, 0, 0, 0);

            }

            i++;
        }
    }
}
