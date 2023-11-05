using Sources.Application.StateMachine.GameStates;
using Sources.Application.StateMachineInterfaces;
using Sources.Application.UI;
using Sources.DIService;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories;
using Sources.Infrastructure.Factories.Player;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.DTO;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Scene;
using Sources.PresentationInterfaces;
using Sources.Services;
using Sources.Services.Interfaces;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.UI;

namespace Sources.Application.StateMachine
{
	public sealed class InitializeServicesWithViewState : IGameState
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly ServiceLocator _serviceLocator;

		private bool _isServicesInitialized;

		public InitializeServicesWithViewState
		(
			GameStateMachine gameStateMachine,
			ServiceLocator serviceLocator
		)
		{
			_gameStateMachine = gameStateMachine;
			_serviceLocator = serviceLocator;

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
			#region GettingServices

			IAssetProvider assetProvider = _serviceLocator.Get<IAssetProvider>();
			IPlayerFactory playerFactory = _serviceLocator.Get<IPlayerFactory>();
			IUpgradeDataFactory upgradeDataFactory = _serviceLocator.Get<IUpgradeDataFactory>();
			IPersistentProgressService progressService = _serviceLocator.Get<IPersistentProgressService>();
			IShopProgressProvider shopProgressProvider = _serviceLocator.Get<IShopProgressProvider>();
			IPlayerProgressProvider playerProgressProvider = _serviceLocator.Get<IPlayerProgressProvider>();
			IResourcesProgressPresenter resourcesProgressPresenter = _serviceLocator.Get<IResourcesProgressPresenter>();

			#endregion

			UIFactory uiFactory = new UIFactory
			(
				assetProvider,
				resourcesProgressPresenter as IResourceProgressEventHandler,
				progressService
			);

			_serviceLocator.Register<IUIFactory>(uiFactory);
			_serviceLocator.Register<IUIGetter>(uiFactory);

			CreateUpgradeWindowService
			(
				assetProvider,
				upgradeDataFactory,
				resourcesProgressPresenter,
				progressService,
				shopProgressProvider,
				playerProgressProvider
			);

			CreateCameraService(assetProvider, playerFactory);

			_isServicesInitialized = true;
		}

		private void CreateUpgradeWindowService
		(
			IAssetProvider assetProvider,
			IUpgradeDataFactory upgradeDataFactory,
			IResourcesProgressPresenter resourcesProgressPresenter,
			IPersistentProgressService progressService,
			IShopProgressProvider shopProgressProvider,
			IPlayerProgressProvider playerProgressProvider
		)
		{
			UpgradeWindowFactory upgradeWindowFactory = new
			(
				assetProvider,
				upgradeDataFactory,
				resourcesProgressPresenter,
				progressService.GameProgress,
				shopProgressProvider,
				playerProgressProvider
			);

			_serviceLocator.Register<IUpgradeWindowFactory>(upgradeWindowFactory);
			_serviceLocator.Register<IUpgradeWindowGetter>(upgradeWindowFactory);
		}

		private void CreateCameraService(IAssetProvider assetProvider, IPlayerFactory playerFactory)
		{
			CameraFactory cameraFactory = new CameraFactory(assetProvider, playerFactory);
			_serviceLocator.Register<ICameraFactory>(cameraFactory);
			_serviceLocator.Register<ICamera>(cameraFactory);
		}
	}
}