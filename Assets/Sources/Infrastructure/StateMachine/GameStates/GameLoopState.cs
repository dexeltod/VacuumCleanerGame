using System;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Adapters;
using Sources.Infrastructure.Providers;
using Sources.Infrastructure.Yandex;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.States.StateMachineInterfaces;
using Sources.Presentation.SceneEntity;
using Sources.ServicesInterfaces;
using UnityEngine;
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
		private readonly IPersistentProgressServiceProvider _persistentProgressServiceProvider;
#if YANDEX_CODE

#endif

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
			AdvertisementHandlerProvider advertisementHandlerProvider,
			IPersistentProgressServiceProvider persistentProgressServiceProvider
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
			_persistentProgressServiceProvider = persistentProgressServiceProvider ??
				throw new ArgumentNullException(nameof(persistentProgressServiceProvider));

			_loadingCurtain = loadingCurtain ? loadingCurtain : throw new ArgumentNullException(nameof(loadingCurtain));
		}

		private IDissolveShaderViewController DissolveShaderViewController =>
			_dissolveShaderViewControllerProvider.Self;

		private IResourcesProgressPresenter ResourcesProgressPresenter => _resourcesProgressPresenterProvider.Self;

		public void Enter()
		{
			SetMoreMoney();

			_localizationService.UpdateTranslations();

			_upgradeWindowPresenterProvider.Self.Enable();
			_gameplayInterfacePresenterProvider.Self.Enable();
			_resourcesProgressPresenterProvider.Self.Enable();
			_gameMenuPresenterProvider.Self.Enable();
			_advertisementHandlerProvider.Self.Enable();

			_loadingCurtain.HideSlowly();
			DissolveShaderViewController.StartDissolving();
		}

		public void Exit()
		{
			_upgradeWindowPresenterProvider.Self.Disable();
			_gameplayInterfacePresenterProvider.Self.Disable();
			_resourcesProgressPresenterProvider.Self.Disable();
			_gameMenuPresenterProvider.Self.Disable();
			_loadingCurtain.Show();
			_advertisementHandlerProvider.Self.Disable();
		}

		private void SetMoreMoney()
		{
#if DEV
			(_persistentProgressServiceProvider.Self.GlobalProgress.ResourceModelReadOnly as IResourceModel)!
				.AddMoney(999999);
#endif
		}
	}
}