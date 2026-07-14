using UnityEngine;
using UnityEngine.SceneManagement;

// Alexander Gottuso

public class MoveButton : MonoBehaviour
{
	[SerializeField] private string nextScene;

	public void OnClick()
	{
		SceneManager.LoadScene(nextScene);
	}
}
