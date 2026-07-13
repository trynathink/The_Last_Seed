using UnityEngine;

// Alexander Gottuso

public class LivingRoomButtonManager : MonoBehaviour
{
	[SerializeField] private GameObject closetClosed;
	[SerializeField] private GameObject closetOpen;

	public void Closet()
	{
		closetOpen.SetActive(true);
		closetClosed.SetActive(false);
	}

	public void BedroomDoor()
	{
		Debug.Log("Dialogue for bedroom door");
	}
}
