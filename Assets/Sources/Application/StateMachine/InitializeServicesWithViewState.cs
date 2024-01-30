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
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.UI;
using VContainer;
using UpgradeWindowFactory = Sources.Application.UI.UpgradeWindowFactory;

namespace Sources.Application.StateMachine
{
	public sealed class InitializeServicesWithViewState : IGameState
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly IAssetProvider _assetProvider;
		private readonly IPlayerFactory _playerFactory;
		private readonly IProgressUpgradeFactory _progressUpgradeFactory;
		private readonly IPersistentProgressService _progressService;
		private readonly IShopProgressProvider _shopProgressProvider;
		private readonly IPlayerProgressProvider _playerProgressProvider;
		private readonly IResourcesProgressPresenter _resourcesProgressPresenter;
		private readonly ICameraFactory _cameraFactory;
		private readonly ITranslatorService _translatorService;

		private bool _isServicesInitialized;

		[Inject]
		public InitializeServicesWithViewState(
			GameStateMachine gameStateMachine,
			IAssetProvider assetProvider,
			IPlayerFactory playerFactory,
			IProgressUpgradeFactory progressUpgradeFactory,
			IPersistentProgressService progressService,
			IShopProgressProvider shopProgressProvider,
			IPlayerProgressProvider playerProgressProvider,
			IResourcesProgressPresenter resourcesProgressPresenter,
			ICameraFactory cameraFactory,
			ITranslatorService translatorService
		)
		{
			_gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
			_assetProvider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));
			_playerFactory = playerFactory ?? throw new ArgumentNullException(nameof(playerFactory));
			_progressUpgradeFactory = progressUpgradeFactory ??
				throw new ArgumentNullException(nameof(progressUpgradeFactory));
			_progressService = progressService ?? throw new ArgumentNullException(nameof(progressService));
			_shopProgressProvider
				= shopProgressProvider ?? throw new ArgumentNullException(nameof(shopProgressProvider));
			_playerProgressProvider = playerProgressProvider ??
				throw new ArgumentNullException(nameof(playerProgressProvider));
			_resourcesProgressPresenter = resourcesProgressPresenter ??
				throw new ArgumentNullException(nameof(resourcesProgressPresenter));
			_cameraFactory = cameraFactory ?? throw new ArgumentNullException(nameof(cameraFactory));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));

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

			CreateUpgradeWindowService();

			CreateCameraService(_assetProvider, _playerFactory);

			_isServicesInitialized = true;
		}

		private void CreateUpgradeWindowService()
		{
			UpgradeWindowFactory upgradeWindowFactory = new(
				_assetProvider,
				_progressUpgradeFactory,
				_resourcesProgressPresenter,
				_progressService,
				_shopProgressProvider,
				_playerProgressProvider,
				_translatorService
			);
		}

		private void CreateCameraService(IAssetProvider assetProvider, IPlayerFactory playerFactory)
		{
			CameraFactory cameraFactory = new CameraFactory(assetProvider, playerFactory);
			// _builder.RegisterInstance<ICameraFactory>(cameraFactory);
			// _builder.RegisterInstance<ICamera>(cameraFactory);
		}
	}
}