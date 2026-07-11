using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Image))]
public class BedroomWindowButtonManager : MonoBehaviour, IPointerClickHandler
{
    public PlayerDataSO PDSO; 

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Debug.Log("back to room");
        PDSO.PlayerLocation = "A1 Bedroom";
        SceneManager.LoadScene(PDSO.PlayerLocation);
    }
}