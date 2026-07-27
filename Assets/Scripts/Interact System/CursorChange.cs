using UnityEngine;
using UnityEngine.EventSystems;

// Gaurav Singh

public class CursorChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeReference]
    PlayerDataSO PDSO;

    [SerializeField]
    Texture2D hover, click;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (PDSO.HeldItem == "")
        {
            Cursor.SetCursor(click, default, default);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(PDSO.HeldItem == "")
        {
            Cursor.SetCursor(hover, default, default);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (PDSO.HeldItem == "")
        {
            Cursor.SetCursor(default, default, default);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (PDSO.HeldItem == "")
        {
            Cursor.SetCursor(hover, default, default);
        }
    }
}
