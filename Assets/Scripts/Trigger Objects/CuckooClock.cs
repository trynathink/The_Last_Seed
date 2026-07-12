using UnityEngine;

public class CuckooClock : ClickableImage, ITriggerObject
{
	[SerializeField] private PlayerDataSO data;

	// TODO: implement this properly with some sound turning off
	public void TriggerEffect()
	{
		Debug.Log("Turned off!");
	}

	public void OnClick()
	{
		TriggerEffect();
		data.triggers.Add(name);
		// potential auto save here
	}
}
