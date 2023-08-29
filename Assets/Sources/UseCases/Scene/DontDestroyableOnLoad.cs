using UnityEngine;

namespace UseCases.Scene
{
	public class DontDestroyableOnLoad : MonoBehaviour
	{
		private void Awake() =>
			DontDestroyOnLoad(this);
	}
}