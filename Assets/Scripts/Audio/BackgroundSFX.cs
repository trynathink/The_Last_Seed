using UnityEngine;

public class BackgroundSFX : AudioSingleton<BackgroundSFX>
{
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
}
