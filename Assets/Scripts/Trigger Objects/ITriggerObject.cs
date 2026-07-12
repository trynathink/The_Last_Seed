// Alexander Gottuso

/* Use this interface for any game object that has a unique effect in a scene,
 * as it relates to what's described for the list `triggers` in PlayerData.cs */

public interface ITriggerObject
{
	public void TriggerEffect();
}
