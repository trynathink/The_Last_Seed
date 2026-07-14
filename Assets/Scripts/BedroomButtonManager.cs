using UnityEngine;
using UnityEngine.SceneManagement;

public class BedroomButtonManager : MonoBehaviour
{
    // Vinayak Karuppasamy

    // This class is responsible for handling all button interactions
    // in the bedroom scene

    public PlayerDataSO PDSO;
    private bool isAlarmOff = false;

    public ScriptsSO AlarmOn, AlarmOff;

    public void BedroomWindow()
    {
        Debug.Log("bedroom window clicked");
        NextScene("A1 Bed Window");
    }

    public void Clock()
    {
        Debug.Log("clock clicked");

        if (!isAlarmOff)
        {
            isAlarmOff = true;
        }
    }

    public void ExitBedroom()
    {
        if(!isAlarmOff)
        {
            GameObject.Find("Dialogue Manager").GetComponent<DialogueManager>().SetLines(AlarmOn);

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