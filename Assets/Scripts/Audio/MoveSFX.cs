using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveSFX : AudioSingleton<MoveSFX>
{
	[SerializeField] private AudioClip _clip; // Can later be an array for each respective transition
	private static AudioClip clip;

	protected override void Awake()
	{
		base.Awake();
		clip = _clip;
	}

    public static void Play()
    {
        source.PlayOneShot(clip);
    }

	// PlayClip doesn't show up as an option for Unity Events
	public static void PlayCustom(AudioClip clip)
	{
		PlayClip(clip);
	}
}
