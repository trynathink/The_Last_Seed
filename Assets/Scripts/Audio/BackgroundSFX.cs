using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundSFX : AudioSingleton<BackgroundSFX>
{
	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "Main Menu" && source != null && source.isPlaying)
		{
			StopLoop();
		}
	}

	public static void PlayLoop(AudioClip clip)
	{
		if (!source.isPlaying)
		{
			source.loop = true;
			PlayClip(clip);
		}
	}

	public static void StopLoop()
	{
		Stop();
		source.loop = false;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
}
