using Sources.ControllersInterfaces;
using UnityEngine;

namespace Sources.PresentationInterfaces.Common
{
	public interface IPresentableView<in T> where T : IPresenter
	{
		Transform Transform { get; }
		void Construct(T presenter);
		void Disable();
		void Enable();
		void SetParent(Transform parent);
	}
}