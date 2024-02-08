using System;
using Sources.Controllers;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Common.Factory;
using Sources.Infrastructure.Common.Factory.Decorators;
using Sources.Infrastructure.Providers;
using Sources.PresentationInterfaces;

namespace Sources.Infrastructure.Factories.Presenters
{
	public class ResourcesProgressPresenterFactory : PresenterFactory<ResourcesProgressPresenter>
	{
		private readonly GameplayInterfaceProvider _gameplayInterfaceProvider;
		private readonly IPersistentProgressService _persistentProgressService;

		private IGameplayInterfaceView GameplayInterfaceView => _gameplayInterfaceProvider.Instance;
		private IResourcesModel Resources => _persistentProgressService.GameProgress.ResourcesModel;

		public ResourcesProgressPresenterFactory(
			GameplayInterfaceProvider gameplayInterfaceProvider,
			IPersistentProgressService persistentProgressService
		)
		{
			_gameplayInterfaceProvider = gameplayInterfaceProvider ??
				throw new ArgumentNullException(nameof(gameplayInterfaceProvider));
			_persistentProgressService = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));
		}

		public override ResourcesProgressPresenter Create() =>
			new ResourcesProgressPresenter(GameplayInterfaceView, Resources);
	}
}