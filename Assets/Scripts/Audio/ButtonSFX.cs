using UnityEngine;
using UnityEngine.UI;

public class ButtonSFX : MonoBehaviour
{
	[SerializeField] private AudioClip clickSound;
	[SerializeField] private AudioSource source;
	[SerializeField] private GameObject canvas;

	private void OnEnable()
	{
		foreach (Button button in canvas.GetComponentsInChildren<Button>(true))
		{
			button.onClick.AddListener(PlaySound);
		}
	}

	private void PlaySound()
	{
		source.PlayOneShot(clickSound);
	}
}
