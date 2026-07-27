using UnityEngine;

public class CollectibleSFX : AudioSingleton<CollectibleSFX>
{
	[SerializeField] private AudioClip collectSound;

    private void OnEnable()
	{
		Collectible.OnCollect += Play;
    }

	private void Play()
	{
		source.PlayOneShot(collectSound);
	}

	private void OnDisable()
	{
		Collectible.OnCollect -= Play;
	}
}
