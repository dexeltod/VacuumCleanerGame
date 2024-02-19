using Sources.ControllersInterfaces;
using Sources.PresentationInterfaces.Common;
using UnityEngine.UI;

namespace Sources.PresentationInterfaces
{
	public interface IGameMenuView : IPresentableView<IGameMenuPresenter>
	{
		public Button OpenMenuButton { get; }
	}
}