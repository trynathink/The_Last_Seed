using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveSFX : MonoBehaviour
{
	[SerializeField] private AudioClip clip; // Can later be an array for each respective transition
	[SerializeField] private AudioSource source;

	public bool door;

    void Awake()
    {
		if(GameObject.FindObjectsByType<MoveSFX>().Length > 1)
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
		SceneManager.sceneLoaded += OnSceneLoaded;	
	}

    public void Play()
    {
        source.PlayOneShot(clip);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (door)
		{
			source.PlayOneShot(clip);

			door = false;
		}
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
}
