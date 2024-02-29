using Sources.ControllersInterfaces.Common;
using UnityEngine;

namespace Sources.PresentationInterfaces.Common
{
	public interface IPresentableView<in T> where T : class, IPresenter
	{
		void Construct(T presenter);
		void Enable();
		void Disable();
		void SetParent(Transform parent);
	}
}