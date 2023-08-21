using UnityEngine;

public class DontDestroyableOnLoad : MonoBehaviour
{
	private void Awake() =>
		DontDestroyOnLoad(this);
}