using UnityEngine;
using UnityEngine.SceneManagement;

// Alexander Gottuso

/* Put this in an empty game object in each scene that the player can move to
 * so that we can correctly load the state of that scene based off what the player has already done */

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private PlayerDataSO data;
	
	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		data.PlayerLocation = scene.name;
		// potential for an auto-save here

		// NOTE: This will be much less expensive if we have a list of potential affected items
		// and a list of potential trigger game objects serialized at the top of this script,
		// but for ease of use we can try like this
		foreach (string item in data.Inventory)
		{
			Destroy(GameObject.Find(item));
		}

		foreach (string trigger in data.triggers)
		{
			if (GameObject.Find(trigger).TryGetComponent(out ITriggerObject obj))
			{
				obj.TriggerEffect();
			}
		}
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
}
