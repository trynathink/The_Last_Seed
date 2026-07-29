using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// Gaurav Singh

public class CursorChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    PlayerDataSO PDSO;

    [SerializeField]
    Texture2D hover, click;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (PDSO.HeldItem == "" || SceneManager.GetActiveScene().name == "Main Menu")
        {
            Cursor.SetCursor(click, default, default);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(PDSO.HeldItem == "" || SceneManager.GetActiveScene().name == "Main Menu")
        {
            Cursor.SetCursor(hover, default, default);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (PDSO.HeldItem == "" || SceneManager.GetActiveScene().name == "Main Menu")
        {
            Cursor.SetCursor(default, default, default);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (PDSO.HeldItem == "" || SceneManager.GetActiveScene().name == "Main Menu")
        {
            Cursor.SetCursor(hover, default, default);
        }
    }
}
