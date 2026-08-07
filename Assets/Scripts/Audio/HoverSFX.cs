using UnityEngine;
using UnityEngine.EventSystems;

public class HoverSFX : MonoBehaviour, IPointerEnterHandler
{
	[SerializeField] private AudioClip clip;
	[SerializeField] private AudioSource source;

	public void OnPointerEnter(PointerEventData eventData)
    {
		source.PlayOneShot(clip);
    }
}
