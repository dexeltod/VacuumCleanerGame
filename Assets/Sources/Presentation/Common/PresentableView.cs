using System;
using Sources.Presentation.Implementation;
using Sources.PresentationInterfaces.Common;
using Sources.PresentersInterfaces;

namespace Sources.Presentation.Common
{
	public abstract class PresentableView<T> : View, IPresentableView<T> where T : class, IPresenter
	{
		protected T Presenter { get; private set; }

		private void OnEnable() =>
			Presenter?.Enable();

		private void OnDisable() =>
			Presenter?.Disable();

		public void Construct(T presenter)
		{
			if (presenter == null)
				throw new ArgumentNullException(nameof(presenter));

			Disable();
			Presenter = presenter;
			Enable();
		}

	}
}