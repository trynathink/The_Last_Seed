using UnityEngine;

public class AudioSingleton<T> : MonoBehaviour where T : UnityEngine.Object
{
	protected static AudioSource source;

    protected virtual void Awake()
	{
		if (GameObject.FindObjectsByType<T>().Length > 1)
        {
			Debug.Log(GameObject.FindObjectsByType<T>());
            GameObject.Destroy(gameObject);
        }
        else
        {
			source = gameObject.GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
	}

	public static void PlayClip(AudioClip clip)
	{
		source.clip = clip;
		source.Play();
	}

	public static void Stop()
	{
		source.Stop();
	}
}
