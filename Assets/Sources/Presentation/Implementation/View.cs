using UnityEngine;

namespace Sources.Presentation.Implementation
{
	public abstract class View : MonoBehaviour
	{
		public void Enable() =>
			gameObject.SetActive(true);

		public void Disable() =>
			gameObject.SetActive(false);
	}
}