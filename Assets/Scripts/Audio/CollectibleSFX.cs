using UnityEngine;

public class CollectibleSFX : MonoBehaviour
{
	[SerializeField] private AudioSource source;
	[SerializeField] private AudioClip collectSound;

	static GameObject Csfx;

	private void OnEnable()
	{
		Collectible.OnCollect += Play;

        DontDestroyOnLoad(gameObject);

        if (Csfx == null)
        {
            Csfx = gameObject;
        }
        else
        {
            Object.Destroy(gameObject);
        }
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
