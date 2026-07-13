using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

// Alexander Gottuso

/* For now, this is designed to use one background song per scene in the build index,
 * but this won't be the final design since there are no transitions implemented yet. */

public class BackgroundMusic : MonoBehaviour
{
	[SerializeField] private List<AudioClip> backgroundSongs;
	[SerializeField] private AudioSource source;

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.buildIndex < backgroundSongs.Count)
		{
			source.clip = backgroundSongs[scene.buildIndex];
			source.Play();
		}
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}
}
