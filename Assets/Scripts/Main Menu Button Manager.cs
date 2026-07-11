using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonManager : MonoBehaviour
{
    // Gaurav Singh

    // Contains the buttons methods for the Main Menu
    // Acts as the Manager for the Main Menu Screen

    public PlayerDataSO PDSO;

    GameObject TitleButtons, SaveButtons;

    void Start()
    {
        TitleButtons = GameObject.Find("Title Screen");
        SaveButtons = GameObject.Find("Saves Screens");

        TitleButtons.SetActive(true);
        SaveButtons.SetActive(false);
    }

    public void QuitButton()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void PlayButton()
    {
        TitleButtons.SetActive(false);
        SaveButtons.SetActive(true);
    }

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

    }
}
