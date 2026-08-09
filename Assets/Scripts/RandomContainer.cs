using UnityEngine;

[CreateAssetMenu(fileName = "RandomContainer", menuName = "Scriptable Objects/RandomContainer")]
public class RandomContainer : ScriptableObject
{
	[SerializeField] private GameObject[] objects;

	public GameObject Get()
	{
		return objects[Random.Range(0, objects.Length)];
	}
}
