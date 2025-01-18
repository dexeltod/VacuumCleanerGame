using Sources.ControllersInterfaces;
using UnityEngine;

namespace Sources.PresentationInterfaces.Common
{
	public interface IPresentableView<in T> where T : IPresenter
	{
		void Construct(T presenter);
		void Enable();
		void Disable();
		void SetParent(Transform parent);
		Transform Transform { get; }
	}
}
