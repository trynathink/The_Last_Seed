using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ClickableImage : MonoBehaviour, IPointerClickHandler
{

    // Vinayak Karuppasamy

    // This script can be attached to sprites we want to add click behaviour to
    // and `OnPointerClick` will get triggered if a click event is registered on
    // the non transparent pixels of that image

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