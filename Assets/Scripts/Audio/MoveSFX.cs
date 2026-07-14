using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveSFX : MonoBehaviour
{
	[SerializeField] private AudioClip clip; // Can later be an array for each respective transition
	[SerializeField] private AudioSource source;

	static GameObject Msfx; 

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;

        DontDestroyOnLoad(gameObject);

        if (Msfx == null)
		{
			Msfx = gameObject;
		}
		else
		{
			Object.Destroy(gameObject);
		}
		
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "A1 Living Room")
		{
			source.PlayOneShot(clip);
		}
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
}
