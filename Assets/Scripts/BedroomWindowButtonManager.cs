using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Gaurav Singh

[RequireComponent(typeof(Image))]
public class BedroomWindowButtonManager : MonoBehaviour
{
    // Vinayak Karuppasamy
    // Gaurav Singh (this may be a small ship of theseus moment)
    
    // This class manages button clicks for the window scene

    // this may be a small 

    [SerializeField]
    AudioSource alarm;

    public PlayerDataSO PDSO;


    private void OnEnable()
    {
        if (PDSO.triggers.Contains("Clock"))
        {
            Clock();
        }
    }

    public void Clock()
    {
        alarm.Stop();
    }

    public void ExitWindow()
    {
        NextScene("A1 Bedroom");
    }

    private void NextScene(string sceneName)
    {
        PDSO.PlayerLocation = sceneName;
        SceneManager.LoadScene(PDSO.PlayerLocation);
    }
}