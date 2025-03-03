using System;
using Sources.PresentationInterfaces.Common;
using UnityEngine;

namespace Sources.Presentation.UI
{
	public abstract class View : MonoBehaviour, IView
	{
		public virtual void Enable() => gameObject.SetActive(true);

		public virtual void Disable() => gameObject.SetActive(false);
		public virtual event Action Enabled;
		public virtual event Action Disabled;
	}
}
