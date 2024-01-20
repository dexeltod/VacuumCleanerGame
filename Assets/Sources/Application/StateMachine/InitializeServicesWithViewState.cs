using System;
using Sources.Application.StateMachine.GameStates;
using Sources.Application.UI;
using Sources.ApplicationServicesInterfaces.StateMachineInterfaces;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories;
using Sources.Infrastructure.Factories.Player;
using Sources.InfrastructureInterfaces.DTO;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Scene;
using Sources.Services;
using Sources.Services.Interfaces;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.UI;
using VContainer;

namespace Sources.Application.StateMachine
{
	public sealed class InitializeServicesWithViewState : IGameState
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly IAssetProvider _assetProvider;
		private readonly IPlayerFactory _playerFactory;
		private readonly IUpgradeDataFactory _upgradeDataFactory;
		private readonly IPersistentProgressService _progressService;
		private readonly IShopProgressProvider _shopProgressProvider;
		private readonly IPlayerProgressProvider _playerProgressProvider;
		private readonly IResourcesProgressPresenter _resourcesProgressPresenter;
		private readonly ICameraFactory _cameraFactory;

		private bool _isServicesInitialized;

		public InitializeServicesWithViewState(
			GameStateMachine gameStateMachine,
			IAssetProvider assetProvider,
			IPlayerFactory playerFactory,
			IUpgradeDataFactory upgradeDataFactory,
			IPersistentProgressService progressService,
			IShopProgressProvider shopProgressProvider,
			IPlayerProgressProvider playerProgressProvider,
			IResourcesProgressPresenter resourcesProgressPresenter,
			ICameraFactory cameraFactory
		)
		{
			_gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
			_assetProvider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));
			_playerFactory = playerFactory ?? throw new ArgumentNullException(nameof(playerFactory));
			_upgradeDataFactory = upgradeDataFactory ?? throw new ArgumentNullException(nameof(upgradeDataFactory));
			_progressService = progressService ?? throw new ArgumentNullException(nameof(progressService));
			_shopProgressProvider
				= shopProgressProvider ?? throw new ArgumentNullException(nameof(shopProgressProvider));
			_playerProgressProvider = playerProgressProvider ??
				throw new ArgumentNullException(nameof(playerProgressProvider));
			_resourcesProgressPresenter = resourcesProgressPresenter ??
				throw new ArgumentNullException(nameof(resourcesProgressPresenter));
			_cameraFactory = cameraFactory ?? throw new ArgumentNullException(nameof(cameraFactory));

			_isServicesInitialized = false;
		}

		public void Exit() { }

		public void Enter()
		{
			if (_isServicesInitialized == true)
			{
				_gameStateMachine.Enter<BuildSceneState>();
				return;
			}

			InitializeServices();

			_gameStateMachine.Enter<BuildSceneState>();
		}

		private void InitializeServices()
		{
			UIFactory uiFactory = new UIFactory(
				_assetProvider,
				_resourcesProgressPresenter as IResourceProgressEventHandler,
				_progressService
			);

			// _builder.RegisterInstance<IUIFactory>(uiFactory);
			// _builder.RegisterInstance<IUIGetter>(uiFactory);

			CreateUpgradeWindowService(
				_assetProvider,
				_upgradeDataFactory,
				_resourcesProgressPresenter,
				_progressService,
				_shopProgressProvider,
				_playerProgressProvider
			);

			CreateCameraService(_assetProvider, _playerFactory);

			_isServicesInitialized = true;
		}

		private void CreateUpgradeWindowService(
			IAssetProvider assetProvider,
			IUpgradeDataFactory upgradeDataFactory,
			IResourcesProgressPresenter resourcesProgressPresenter,
			IPersistentProgressService progressService,
			IShopProgressProvider shopProgressProvider,
			IPlayerProgressProvider playerProgressProvider
		)
		{
			UpgradeWindowFactory upgradeWindowFactory = new(
				assetProvider,
				upgradeDataFactory,
				resourcesProgressPresenter,
				progressService.GameProgress,
				shopProgressProvider,
				playerProgressProvider
			);

			// _builder.RegisterInstance<IUpgradeWindowFactory>(upgradeWindowFactory);
			// _builder.RegisterInstance<IUpgradeWindowGetter>(upgradeWindowFactory);
		}

		private void CreateCameraService(IAssetProvider assetProvider, IPlayerFactory playerFactory)
		{
			CameraFactory cameraFactory = new CameraFactory(assetProvider, playerFactory);
			// _builder.RegisterInstance<ICameraFactory>(cameraFactory);
			// _builder.RegisterInstance<ICamera>(cameraFactory);
		}
	}
}