using UnityEngine;

public class CollectibleSFX : MonoBehaviour
{
	[SerializeField] private AudioSource source;
	[SerializeField] private AudioClip collectSound;

    void Awake()
    {
        if (GameObject.FindObjectsByType<CollectibleSFX>().Length > 1)
        {
            GameObject.Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

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
