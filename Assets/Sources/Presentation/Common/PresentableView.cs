using System;
using Sources.ControllersInterfaces.Common;
using Sources.Presentation.UI;
using Sources.PresentationInterfaces.Common;
using UnityEngine;

namespace Sources.Presentation.Common
{
	public abstract class PresentableView<T> : View, IPresentableView<T> where T : class, IPresenter
	{
		private bool _isEnabled;
		protected T Presenter { get; set; }

		private void OnEnable()
		{
			if (!_isEnabled)
				Presenter?.Enable();
		}

		protected virtual void DestroySelf() =>
			Destroy(gameObject);

		private void OnDestroy() =>
			DestroySelf();

		private void OnDisable()
		{
			if (_isEnabled)
				Presenter?.Disable();
		}

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