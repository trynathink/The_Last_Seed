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


    void Awake()
    {
        hover = PDSO.CursorHover;
        click = PDSO.CursorClick;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (PDSO.HeldItem == "" || SceneManager.GetActiveScene().name == "Main Menu")
        {
            Cursor.SetCursor(click, default, CursorMode.ForceSoftware);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(PDSO.HeldItem == "" || SceneManager.GetActiveScene().name == "Main Menu")
        {
            Cursor.SetCursor(hover, default, CursorMode.ForceSoftware);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (PDSO.HeldItem == "" || SceneManager.GetActiveScene().name == "Main Menu")
        {
            Cursor.SetCursor(default, default, CursorMode.ForceSoftware);
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
