using System;
using Application.DI;
using Cysharp.Threading.Tasks;
using Infrastructure.Factories;
using InfrastructureInterfaces;

namespace Infrastructure.StateMachine.GameStates
{
	public class LoadProgressState : IGameState
	{
		private readonly GameStateMachine _gameStateMachine;

		private ProgressFactory _progressFactory;
		private IPersistentProgressService _persistentProgress;
		private ISaveLoadDataService _saveLoadService;

		public LoadProgressState(GameStateMachine gameStateMachine)
		{
			_gameStateMachine = gameStateMachine;
		}

		public async void Enter()
		{
			_saveLoadService = ServiceLocator.Container.GetSingle<ISaveLoadDataService>();
			_persistentProgress = ServiceLocator.Container.GetSingle<IPersistentProgressService>();
			_progressFactory ??= new ProgressFactory(_saveLoadService, _persistentProgress);
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
			await _progressFactory.UpdateProgress();
			progressLoaded.Invoke();
		}
	}
}