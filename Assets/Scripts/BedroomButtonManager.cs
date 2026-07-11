using UnityEngine;
using UnityEngine.SceneManagement;

public class BedroomButtonManager : MonoBehaviour
{
    // Vinayak Karuppasamy

    // This class is responsible for handling all button interactions
    // in the bedroom scene

    public PlayerDataSO PDSO;

    public void BedroomWindow()
    {
        Debug.Log("bedroom window clicked");
        PDSO.PlayerLocation = "A1 Bed Window";
        SceneManager.LoadScene(PDSO.PlayerLocation);
    }

    public void Blanket()
    {
        Debug.Log("blanket clicked");
        // will add inventory code here and maybe make this more general later
    }
}