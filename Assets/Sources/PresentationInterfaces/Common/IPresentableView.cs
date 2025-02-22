using Sources.ControllersInterfaces;
using UnityEngine;

namespace Sources.PresentationInterfaces.Common
{
	public interface IPresentableView<in T> : IView where T : IPresenter
	{
		Transform Transform { get; }
		void Construct(T presenter);

		void SetParent(Transform parent);
	}

	public interface IPresentableView : IView
	{
		Transform Transform { get; }
		void Construct(IPresenter presenter);

		void SetParent(Transform parent);
	}
}
