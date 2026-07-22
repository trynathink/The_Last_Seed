using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveSFX : AudioSingleton<MoveSFX>
{
	[SerializeField] private AudioClip clip; // Can later be an array for each respective transition

	public bool door;

    private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;	
	}

    public void Play()
    {
        source.PlayOneShot(clip);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (door)
		{
			source.PlayOneShot(clip);
			door = false;
		}
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
}
