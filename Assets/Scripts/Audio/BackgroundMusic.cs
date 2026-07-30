using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

// Alexander Gottuso

/* For now, this is designed to use one background song per scene,
 * but this won't be the final design since there are no transitions implemented yet. */

public class BackgroundMusic : AudioSingleton<BackgroundMusic>
{
	const float fadeTime = 1f; // Time in seconds for fade in and out respectively

	// NOTE: This override assumes that the clip on the AudioSource for this BGM contains that scene's song
	protected override void Awake()
	{
		AudioSource current = gameObject.GetComponent<AudioSource>();

		if (source != null && current.clip != source.clip)
		{
			current.Stop();
			StartCoroutine(Fade(current.clip));
		}
		else
		{
			base.Awake();
		}
	}

	private IEnumerator Fade(AudioClip next)
	{
		float startVolume = source.volume;

        while (source.volume > 0)
		{
            source.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        source.Stop();
		source.clip = next;
		source.Play();

        while (source.volume < 1)
		{
            source.volume += Time.deltaTime / fadeTime;
            yield return null;
        }

		base.Awake();
	}
}
