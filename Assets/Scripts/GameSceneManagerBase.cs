using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManagerBase : MonoBehaviour
{
	[SerializeField] protected PlayerDataSO PDSO;

    protected DialogueManager DM;
    protected InventoryManager IM;

	public ScriptsSO DefaultItemFail;

	protected virtual void OnEnable()
	{
        DM = GameObject.Find("Dialogue Manager").GetComponent<DialogueManager>();
        IM = GameObject.Find("Inventory").GetComponent<InventoryManager>();
	}

	protected void CatchAnyItemHeld(Action actionIfNoneHeld)
	{
		if (PDSO.HeldItem == "")
			actionIfNoneHeld();
		else
			DM.SetLines(DefaultItemFail);
	}

	// All scenes
    public void Item(ScriptsSO script)
    {
        IM.HoldItem("");
		CatchAnyItemHeld(() => DM.SetLines(script));
    }

	public void addTrigger(string t)
    {
        if (!PDSO.triggers.Contains(t))
        {
            PDSO.triggers.Add(t);
        }
    }

    public virtual void NextScene(string sceneName)
    {
        PDSO.PlayerLocation = sceneName;
        SceneManager.LoadScene(PDSO.PlayerLocation);
    }
}
