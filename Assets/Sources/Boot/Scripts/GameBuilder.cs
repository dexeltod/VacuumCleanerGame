using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sources.Boot.Scripts.Factories.StateMachine;
using Sources.BusinessLogic.Interfaces.Factory;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.Services;
using Sources.BusinessLogic.States;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Sources.Boot.Scripts
{
	public class GameBuilder : IAsyncStartable
	{
		private readonly GameStatesRepositoryInitializer _gameStatesRepositoryInitializer;
		private readonly IProgressFactory _progressFactory;
		private readonly GameStateMachineRepository _repository;
		private readonly ISaveLoader _saveLoader;
		private readonly IStateMachine _stateMachine;
		private readonly IUpdatablePersistentProgressService _updatablePersistentProgressService;

		[Inject]
		public GameBuilder(
			IProgressFactory progressFactory,
			ISaveLoader saveLoader,
			IStateMachine stateMachineProvider,
			IUpdatablePersistentProgressService updatablePersistentProgressService,
			GameStatesRepositoryInitializer gameStatesRepositoryInitializer,
			GameStateMachineRepository gameStateMachineRepository
		)
		{
			_progressFactory = progressFactory ?? throw new ArgumentNullException(nameof(progressFactory));
			_saveLoader = saveLoader ?? throw new ArgumentNullException(nameof(saveLoader));
			_stateMachine = stateMachineProvider ?? throw new ArgumentNullException(nameof(stateMachineProvider));

			_updatablePersistentProgressService =
				updatablePersistentProgressService ?? throw new ArgumentNullException(nameof(updatablePersistentProgressService));
			_gameStatesRepositoryInitializer = gameStatesRepositoryInitializer
			                                   ?? throw new ArgumentNullException(nameof(gameStatesRepositoryInitializer));
			_repository =
				gameStateMachineRepository ?? throw new ArgumentNullException(nameof(gameStateMachineRepository));
		}

		public async UniTask StartAsync(CancellationToken cancellation)
		{
			Debug.Log("test");

			_gameStatesRepositoryInitializer.Initialize(_repository);
			await Initialize();

			DOTween.Init();

			_stateMachine.Enter<IMenuState>();
		}

		private async UniTask Initialize()
		{
			await _saveLoader.Initialize();

			IGlobalProgress progress = await _progressFactory.Create();

			_updatablePersistentProgressService.Update(progress);
		}
	}
}