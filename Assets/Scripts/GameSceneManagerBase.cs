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

    // This does not work, Item interactions that should not work are just going through,
    // I am going to cut this out for this build
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
        switch (PDSO.HeldItem)
        {
            case "":
                DM.SetLines(script);
                break;
            default:
                DM.SetLines(DefaultItemFail);
                break;
        }

        IM.HoldItem("");

    }

	public virtual void addTrigger(string t)
    {
        if (!PDSO.triggers.Contains(t))
        {
            PDSO.triggers.Add(t);
        }
    }

    public virtual void NextScene(string sceneName)
    {
        PDSO.HeldItem = "";
        PDSO.PlayerLocation = sceneName;
        SceneManager.LoadScene(PDSO.PlayerLocation);
    }
}
