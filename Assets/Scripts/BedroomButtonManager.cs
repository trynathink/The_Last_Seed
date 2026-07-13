using UnityEngine;
using UnityEngine.SceneManagement;

public class BedroomButtonManager : MonoBehaviour
{
    // Vinayak Karuppasamy

    // This class is responsible for handling all button interactions
    // in the bedroom scene

    public PlayerDataSO PDSO;
    private bool isAlarmOff = false;

    public void BedroomWindow()
    {
        Debug.Log("bedroom window clicked");
        NextScene("A1 Bed Window");
    }

    public void Blanket()
    {
        Debug.Log("blanket clicked");
        // will add inventory code here and maybe make this more general later
    }

    public void Clock()
    {
        Debug.Log("clock clicked");

        if (!isAlarmOff)
        {
            // turn alarm off
        }
    }

    public void ExitBedroom()
    {
        if(!isAlarmOff)
        {
            // disable alarm dialogue
            return;
        }

        NextScene("A1 Living Room");
    }

    private void NextScene(string sceneName)
    {
        PDSO.PlayerLocation = sceneName;
        SceneManager.LoadScene(PDSO.PlayerLocation);
    }
}