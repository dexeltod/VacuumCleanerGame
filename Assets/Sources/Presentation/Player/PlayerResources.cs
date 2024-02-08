using Sources.InfrastructureInterfaces.Presenters;
using Sources.PresentationInterfaces.Player;
using VContainer;

namespace Sources.Presentation.Player
{
	public class PlayerResources : IPlayerResources
	{
		private IResourcesProgressPresenter _resourcesProgress;

		[Inject]
		public void Construct(IResourcesProgressPresenter resourcesProgressPresenter) =>
			_resourcesProgress = resourcesProgressPresenter;

		public void SellSand() =>
			_resourcesProgress.SellSand();
	}
}