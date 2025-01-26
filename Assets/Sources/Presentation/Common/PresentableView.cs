using System;
using Sources.ControllersInterfaces;
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

		private void OnDisable()
		{
			if (_isEnabled)
				Presenter?.Disable();
		}

		private void OnDestroy() => DestroySelf();

		public virtual void Construct(T presenter) => Presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));

		public virtual void SetParent(Transform parent) => transform.SetParent(parent);

		public Transform Transform => transform;

		protected virtual void DestroySelf() => Destroy(gameObject);
	}
}