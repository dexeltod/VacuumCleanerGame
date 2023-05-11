using Model.DI;
using Model.Infrastructure.Services;
using Model.Infrastructure.Services.Factories;
using Model.Infrastructure.StateMachine;
using Model.Infrastructure.StateMachine.GameStates;
using ViewModel;
using ViewModel.Infrastructure;

namespace Model.Infrastructure
{
	public class InitializeServicesWithProgressState : IGameState
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly ServiceLocator _serviceLocator;

		public InitializeServicesWithProgressState(GameStateMachine gameStateMachine, ServiceLocator serviceLocator)
		{
			_gameStateMachine = gameStateMachine;
			_serviceLocator = serviceLocator;
		}

		public void Enter()
		{
			RegisterServices();
			_gameStateMachine.Enter<MenuState>();
		}

		public void Exit()
		{
		}

		private void RegisterServices()
		{
			_serviceLocator.RegisterAsSingle<IGameProgressViewModel>(new GameProgressViewModel());
			CreateUI();
			_serviceLocator.RegisterAsSingle<IInputService>(new InputService());
			UpgradeWindowFactory upgradeWindowFactory = new();
			_serviceLocator.RegisterAsSingle<IUpgradeWindowFactory>(upgradeWindowFactory);
			_serviceLocator.RegisterAsSingle<IUpgradeWindowGetter>(upgradeWindowFactory);
			_serviceLocator.RegisterAsSingle<IPlayerFactory>(new PlayerFactory(_serviceLocator.GetSingle<IAssetProvider>()));
		}

		private void CreateUI()
		{
			UIFactory uiFactory = new UIFactory();
			_serviceLocator.RegisterAsSingle<IUIFactory>(uiFactory);
			_serviceLocator.RegisterAsSingle<IUIGetter>(uiFactory);
		}
	}
}