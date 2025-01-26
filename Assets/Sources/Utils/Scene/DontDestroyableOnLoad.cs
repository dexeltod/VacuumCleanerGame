using UnityEngine;

namespace Sources.Utils.Scene
{
	public class DontDestroyableOnLoad : MonoBehaviour
	{
		private void Awake() => DontDestroyOnLoad(this);
	}
}