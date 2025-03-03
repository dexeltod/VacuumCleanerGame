using System;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces.ViewEntities;
using Sources.Presentation.UI;
using Sources.PresentationInterfaces.Common;
using UnityEngine;

namespace Sources.Presentation.Common
{
	public abstract class PresentableView<T> : View, IPresentableView<T> where T : class, IPresenter
	{
		private bool _isEnabled;

		private IViewEntity _viewEntity;
		protected T Presenter { get; set; }

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		protected virtual void OnDestroy()
		{
			DestroySelf();
		}

		public virtual void Construct(T presenter)
		{
			Presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
		}

		public virtual void SetParent(Transform parent) => transform.SetParent(parent);

		public Transform Transform => transform;

		public override event Action Enabled;
		public override event Action Disabled;

		protected virtual void DestroySelf() => Destroy(gameObject);
	}
}
