using UnityEngine;
using UnityEngine.UI;

public class Collectible : MonoBehaviour
{
    // Vinayak Karuppasamy

    // This component allows us to tag an object as a collectible, meaning it can
    // be added to the inventory. Once the object is clicked on, `Collect` can be 
    // triggered by registering it to the onClick of the object.

    public PlayerDataSO PDSO; 

    public void Collect()
    {
        Debug.Log("collect triggered");

        Image image = GetComponent<Image>();
        string imageName = image.sprite.name;
        PDSO.AddToInventory(imageName);
        
        gameObject.SetActive(false);
    }
}