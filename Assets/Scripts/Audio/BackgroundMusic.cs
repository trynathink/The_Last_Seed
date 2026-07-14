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

	static GameObject BGsfx;

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void Awake()
	{
        DontDestroyOnLoad(gameObject);

        if (BGsfx == null)
        {
            BGsfx = gameObject;
        }
        else
        {
            Object.Destroy(gameObject);
        }
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
		SceneManager.sceneLoaded += OnSceneLoaded;
	}
}
