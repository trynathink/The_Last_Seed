using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

// Alexander Gottuso

/* For now, this is designed to use one background song per scene in the build index,
 * but this won't be the final design since there are no transitions implemented yet. */

public class BackgroundMusic : AudioSingleton<BackgroundMusic>
{
	[SerializeField] private List<AudioClip> backgroundSongs;

    private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		// TODO: need a better way to use the same song uninterrupted across multiple scenes, for now this is good enough
		if (scene.buildIndex < backgroundSongs.Count)
		{
			AudioClip clip = backgroundSongs[scene.buildIndex];
			if (source.clip != clip)
			{
				source.clip = backgroundSongs[scene.buildIndex];
				source.Play();
			}
		}
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
}
