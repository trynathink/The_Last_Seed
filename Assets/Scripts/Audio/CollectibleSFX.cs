using UnityEngine;

public class CollectibleSFX : MonoBehaviour
{
	[SerializeField] private AudioSource source;
	[SerializeField] private AudioClip collectSound;

	private void OnEnable()
	{
		Collectible.OnCollect += Play;
		DontDestroyOnLoad(gameObject);
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
