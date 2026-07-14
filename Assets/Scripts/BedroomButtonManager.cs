using UnityEngine;
using UnityEngine.SceneManagement;

public class BedroomButtonManager : MonoBehaviour
{
    // Vinayak Karuppasamy

    // This class is responsible for handling all button interactions
    // in the bedroom scene

    [SerializeField] private PlayerDataSO PDSO;
    [SerializeField] private GameObject blanket;
	[SerializeField] private AudioSource alarm;

    private bool isAlarmOff = false;

    public void BedroomWindow()
    {
        Debug.Log("bedroom window clicked");
        NextScene("A1 Bed Window");
    }

    public void Blanket()
    {
		PDSO.Inventory.Add(blanket.name);
    }

	public void Clock()
	{
		isAlarmOff = true;
		alarm.Stop();
	}

	public void OnClockClick()
	{
		if (!isAlarmOff)
		{
			Clock();
			PDSO.triggers.Add("Clock");
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
