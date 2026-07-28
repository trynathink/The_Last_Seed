using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Vinayak Karuppasamy

// Crafting Area UI operations

public class CraftingAreaManager : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private GameObject leftItemIndicator;
    private Image leftImage;

    [SerializeField]
    private GameObject rightItemIndicator;
    private Image rightImage;

    [SerializeField]
    private Sprite validSprite;

    [SerializeField]
    private Sprite invalidSprite;

    [SerializeField]
    private GameObject craftButton;

    [SerializeField]
    private Vector2 craftingSize = new Vector2(75f, 75f);

    private CraftingManager craftingManager;

    private void Awake()
    {
        TurnOffIndicators();

        leftImage = leftItemIndicator.GetComponent<Image>();
        rightImage = rightItemIndicator.GetComponent<Image>();

        craftingManager = GetComponentInParent<CraftingManager>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        DraggableInventoryItem item = 
            eventData.pointerDrag?.GetComponent<DraggableInventoryItem>();

        if (item == null)
            return;
        
        if(transform.childCount > 1)
            return;

        item.MoveToCraftingArea(transform, craftingSize);
    }

    private void OnTransformChildrenChanged()
    {
        SetValiditySprites();
    }

    private void SetValiditySprites()
    {
        TurnOffIndicators();
        int state = craftingManager.CraftMeter();
        Debug.Log($"crafting state {state}");
        switch (state)
        {
            case 0:
                // left is present but uncombinable
                if(transform.childCount == 1)
                {
                    leftItemIndicator.SetActive(true);
                    leftImage.sprite = invalidSprite;
                }
                else if(transform.childCount == 2)
                {
                    leftItemIndicator.SetActive(true);
                    leftImage.sprite = invalidSprite;
                    rightItemIndicator.SetActive(true);
                    rightImage.sprite = invalidSprite;
                }
                break;
            case 1:
                leftItemIndicator.SetActive(true);
                leftImage.sprite = validSprite;

                // right is present but incompatible
                if(transform.childCount == 2)
                {
                    rightItemIndicator.SetActive(true);
                    rightImage.sprite = invalidSprite;
                }
                break;
            case 2:
                leftItemIndicator.SetActive(true);
                leftImage.sprite = validSprite;

                rightItemIndicator.SetActive(true);
                rightImage.sprite = validSprite;

                craftButton.SetActive(true);
                break;
            default:
                // already turned off the indicators
                break;
        }
    }

    private void TurnOffIndicators()
    {
        leftItemIndicator.SetActive(false);
        rightItemIndicator.SetActive(false);
        craftButton.SetActive(false);
    }
}