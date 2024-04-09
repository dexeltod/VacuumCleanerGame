using System;
using Sources.ControllersInterfaces;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.States.StateMachineInterfaces;
using Sources.Presentation.SceneEntity;
using Sources.ServicesInterfaces;
using VContainer;

namespace Sources.Infrastructure.StateMachine.GameStates
{
	public sealed class GameLoopState : IGameState
	{
		private readonly ILocalizationService _localizationService;
		private readonly UpgradeWindowPresenterProvider _upgradeWindowPresenterProvider;
		private readonly IGameplayInterfacePresenterProvider _gameplayInterfacePresenterProvider;
		private readonly IResourcesProgressPresenterProvider _resourcesProgressPresenterProvider;
		private readonly DissolveShaderViewControllerProvider _dissolveShaderViewControllerProvider;

		private readonly IGameMenuPresenterProvider _gameMenuPresenterProvider;
		private readonly AdvertisementHandlerProvider _advertisementHandlerProvider;
		private readonly LoadingCurtain _loadingCurtain;

		[Inject]
		public GameLoopState(
			LoadingCurtain loadingCurtain,
			ILocalizationService localizationService,
			UpgradeWindowPresenterProvider upgradeWindowPresenterProvider,
			IGameplayInterfacePresenterProvider gameplayInterfacePresenterProvider,
			IResourcesProgressPresenterProvider resourcesProgressPresenterProvider,
			DissolveShaderViewControllerProvider dissolveShaderViewControllerProvider,
			IGameMenuPresenterProvider gameMenuPresenterProvider,
			AdvertisementHandlerProvider advertisementHandlerProvider
		)
		{
			_localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
			_upgradeWindowPresenterProvider
				= upgradeWindowPresenterProvider ??
				throw new ArgumentNullException(nameof(upgradeWindowPresenterProvider));

			_gameplayInterfacePresenterProvider = gameplayInterfacePresenterProvider ??
				throw new ArgumentNullException(nameof(gameplayInterfacePresenterProvider));
			_resourcesProgressPresenterProvider = resourcesProgressPresenterProvider ??
				throw new ArgumentNullException(nameof(resourcesProgressPresenterProvider));
			_dissolveShaderViewControllerProvider = dissolveShaderViewControllerProvider ??
				throw new ArgumentNullException(nameof(dissolveShaderViewControllerProvider));
			_gameMenuPresenterProvider = gameMenuPresenterProvider ??
				throw new ArgumentNullException(nameof(gameMenuPresenterProvider));
			_advertisementHandlerProvider = advertisementHandlerProvider ??
				throw new ArgumentNullException(nameof(advertisementHandlerProvider));
			_loadingCurtain = loadingCurtain ? loadingCurtain : throw new ArgumentNullException(nameof(loadingCurtain));
		}

		private IDissolveShaderViewController DissolveShaderViewController =>
			_dissolveShaderViewControllerProvider.Implementation;

		private IResourcesProgressPresenter ResourcesProgressPresenter =>
			_resourcesProgressPresenterProvider.Implementation;

		public void Enter()
		{
			_localizationService.UpdateTranslations();

			_upgradeWindowPresenterProvider.Implementation.Enable();
			_gameplayInterfacePresenterProvider.Implementation.Enable();
			_resourcesProgressPresenterProvider.Implementation.Enable();
			_gameMenuPresenterProvider.Implementation.Enable();
			_advertisementHandlerProvider.Implementation.Enable();

			_loadingCurtain.HideSlowly();
			DissolveShaderViewController.StartDissolving();
		}

		public void Exit()
		{
			_upgradeWindowPresenterProvider.Implementation.Disable();
			_gameplayInterfacePresenterProvider.Implementation.Disable();
			_resourcesProgressPresenterProvider.Implementation.Disable();
			_gameMenuPresenterProvider.Implementation.Disable();
			_loadingCurtain.Show();
			_advertisementHandlerProvider.Implementation.Disable();
		}
	}
}