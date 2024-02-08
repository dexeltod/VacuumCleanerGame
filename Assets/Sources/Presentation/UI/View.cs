using UnityEngine;

namespace Sources.Presentation.UI
{
	public abstract class View : MonoBehaviour
	{
		public void Enable() =>
			gameObject.SetActive(true);

		public void Disable() =>
			gameObject.SetActive(false);
	}
}