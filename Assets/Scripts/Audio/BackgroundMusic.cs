using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

// Alexander Gottuso

/* For now, this is designed to use one background song per scene,
 * but this won't be the final design since there are no transitions implemented yet. */

public class BackgroundMusic : AudioSingleton<BackgroundMusic>
{
	// NOTE: This override assumes that the clip on the AudioSource for this BGM contains that scene's song
	protected override void Awake()
	{
		AudioClip next = gameObject.GetComponent<AudioSource>().clip;

		if (source != null && next != source.clip)
		{
			source.clip = next;
			source.Play();
		}

		base.Awake();
	}
}
