using System;
using Sources.Controllers;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Common.Factory;
using Sources.Infrastructure.Common.Factory.Decorators;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces.Providers;
using Sources.Presentation.UI;
using Sources.PresentationInterfaces;
using VContainer;

namespace Sources.Infrastructure.Factories.Presenters
{
	public class ResourcesProgressPresenterFactory : PresenterFactory<ResourcesProgressPresenter>
	{
		private readonly IGameplayInterfaceProvider _gameplayInterfaceProvider;
		private readonly IPersistentProgressService _persistentProgressService;
		private readonly ISandContainerViewProvider _sandContainerViewProvider;

		private IResourcesModel Resources => _persistentProgressService.GameProgress.ResourcesModel;

		[Inject]
		public ResourcesProgressPresenterFactory(
			IGameplayInterfaceProvider gameplayInterfaceProvider,
			IPersistentProgressService persistentProgressService,
			ISandContainerViewProvider sandContainerViewProvider
		)
		{
			_gameplayInterfaceProvider = gameplayInterfaceProvider ??
				throw new ArgumentNullException(nameof(gameplayInterfaceProvider));
			_persistentProgressService = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));
			_sandContainerViewProvider = sandContainerViewProvider ??
				throw new ArgumentNullException(nameof(sandContainerViewProvider));
		}

		public override ResourcesProgressPresenter Create() =>
			new ResourcesProgressPresenter(_gameplayInterfaceProvider, Resources, _sandContainerViewProvider);
	}
}