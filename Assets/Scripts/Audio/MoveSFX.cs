using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveSFX : MonoBehaviour
{
	[SerializeField] private AudioClip clip; // Can later be an array for each respective transition
	[SerializeField] private AudioSource source;

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
		DontDestroyOnLoad(gameObject);
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
