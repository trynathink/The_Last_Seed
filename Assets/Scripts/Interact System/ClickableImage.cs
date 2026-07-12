using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ClickableImage : MonoBehaviour, IPointerClickHandler
{
    [Range(0f, 1f)]
    [SerializeField]
    private float alphaThreshold = 0.1f;

    [SerializeField]
    private UnityEvent onClick;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.alphaHitTestMinimumThreshold = alphaThreshold;
        image.raycastTarget = true;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Debug.Log("clickable image on pointer click");
        onClick?.Invoke();
    }
}