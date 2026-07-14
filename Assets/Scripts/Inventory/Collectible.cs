using UnityEngine;
using UnityEngine.UI;
using System;

public class Collectible : MonoBehaviour
{
    // Vinayak Karuppasamy

    // This component allows us to tag an object as a collectible, meaning it can
    // be added to the inventory. Once the object is clicked on, `Collect` can be 
    // triggered by registering it to the onClick of the object.
	
	public static event Action OnCollect;

    [SerializeField]
    private CollectibleType type;
    
    [SerializeField]
    private PlayerDataSO PDSO;

    public void Collect()
    {
        Debug.Log("collect triggered");
        PDSO.AddToInventory(type.ToString());
        gameObject.SetActive(false);
		OnCollect?.Invoke();
    }
}
