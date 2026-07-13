using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DestructibleImage : MonoBehaviour
{

    // Vinayak Karuppasamy

    // This script can be attached to sprites we want to destroy from the scene
    // `OnPointerClick`. This can be conditional based on whether a collectible 
    // is equipped or not.

    [SerializeField]
    private CollectibleType type;

    [SerializeField]
    private bool ignoreCollectible = false;

    public PlayerDataSO PDSO;

    public void Destroy()
    {
        if(ignoreCollectible || PDSO.equippedItem == type.ToString()) {
            Debug.Log("image destroyed");
            Destroy(gameObject);
        }
    }
}