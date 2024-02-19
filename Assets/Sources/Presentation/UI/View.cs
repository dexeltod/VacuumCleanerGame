using UnityEngine;

namespace Sources.Presentation.UI
{
	public abstract class View : MonoBehaviour
	{
		public virtual void Enable() =>
			gameObject.SetActive(true);

		public virtual void Disable() =>
			gameObject.SetActive(false);
	}
}