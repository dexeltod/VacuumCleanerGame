using Sources.InfrastructureInterfaces.Presenters;
using Sources.PresentationInterfaces.Common;

namespace Sources.PresentationInterfaces.Player
{
	public interface IPlayerResources : IPresentableView<IResourcesProgressPresenter>
	{
		void SellSand();
	}
}