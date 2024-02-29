using System;
using Sources.ControllersInterfaces.Common;
using Sources.Presentation.UI;
using Sources.PresentationInterfaces.Common;
using UnityEngine;

namespace Sources.Presentation.Common
{
	public abstract class PresentableView<T> : View, IPresentableView<T> where T : class, IPresenter
	{
		protected T Presenter { get; set; }

		private void OnEnable() =>
			Presenter?.Enable();

		private void OnDisable() =>
			Presenter?.Disable();

		public virtual void Construct(T presenter)
		{
			if (presenter == null)
				throw new ArgumentNullException(nameof(presenter));

			Disable();
			Presenter = presenter;
			Enable();
		}

		public virtual void SetParent(Transform parent) =>
			transform.SetParent(parent);
	}
}