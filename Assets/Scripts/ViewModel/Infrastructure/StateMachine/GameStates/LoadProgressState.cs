using System;
using Cysharp.Threading.Tasks;
using Model;
using Model.DI;
using Model.Infrastructure.Data;
using ViewModel.Infrastructure.Services;
using ViewModel.Infrastructure.Services.Factories;

namespace ViewModel.Infrastructure.StateMachine.GameStates
{
	public class LoadProgressState : IGameState
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly ServiceLocator _serviceLocator;
		private readonly ISaveLoadDataService _saveLoadService;
		private GameProgressModel _gameProgress;

		public LoadProgressState(GameStateMachine gameStateMachine, ServiceLocator serviceLocator)
		{
			_saveLoadService = ServiceLocator.Container.GetSingle<ISaveLoadDataService>();
			_gameStateMachine = gameStateMachine;
			_serviceLocator = serviceLocator;
		}

		public async void Enter()
		{
			await LoadProgressOrInitNew(OnProgressLoaded);
		}

		public void Exit()
		{
		}

		private void OnProgressLoaded()
		{
			_gameStateMachine.Enter<InitializeServicesWithProgressState>();
		}

		private async UniTask LoadProgressOrInitNew(Action progressLoaded)
		{
			await _saveLoadService.LoadProgress();
			progressLoaded.Invoke();
		}
	}
}