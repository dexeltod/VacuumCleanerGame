using UnityEngine;

namespace Sources.UseCases.Scene
{
	public class DontDestroyableOnLoad : MonoBehaviour
	{
		private void Awake() =>
			DontDestroyOnLoad(this);
	}
}