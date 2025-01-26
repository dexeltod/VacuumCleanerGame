using Sources.PresentationInterfaces.Common;
using UnityEngine;

namespace Sources.Presentation.UI
{
	public abstract class View : MonoBehaviour, IView
	{
		public virtual void Enable() => gameObject.SetActive(true);

		public virtual void Disable() => gameObject.SetActive(false);
	}
}