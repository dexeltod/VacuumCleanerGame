using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.Application.Services;
using Sources.Application.StateMachine;
using Sources.Application.StateMachine.GameStates;
using Sources.ApplicationServicesInterfaces;
using Sources.ApplicationServicesInterfaces.StateMachineInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Factories;
using Sources.Infrastructure.Factories.Player;
using VContainer;
using VContainer.Unity;

namespace Sources.Application
{
	public class Game : IAsyncStartable
	{
		private readonly GameStateMachineFactory _gameStateMachineFactory;
		private readonly ProgressFactory _progressFactory;
		private readonly ISaveLoader _saveLoader;
		private readonly IGameStateMachine _gameStateMachine;
		private readonly IYandexSDKController _yandexSDKController;
		
		private GameStateMachine _stateMachine;

		[Inject]
		public Game(
			GameStateMachineFactory gameStateMachineFactory,
			ProgressFactory progressFactory,
			ISaveLoader saveLoader,
			IGameStateMachine gameStateMachine
#if YANDEX_CODE
			, IYandexSDKController yandexSDKController
#endif
		)
		{
			_gameStateMachineFactory = gameStateMachineFactory ??
				throw new ArgumentNullException(nameof(gameStateMachineFactory));

			_progressFactory = progressFactory ?? throw new ArgumentNullException(nameof(progressFactory));
			_saveLoader = saveLoader ?? throw new ArgumentNullException(nameof(saveLoader));
			_gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
#if YANDEX_CODE
			_yandexSDKController = yandexSDKController ?? throw new ArgumentNullException(nameof(yandexSDKController));
#endif
		}

		private async UniTask Initialize()
		{
			_stateMachine = _gameStateMachineFactory.Create((GameStateMachine)_gameStateMachine);
			await _saveLoader.Initialize();
			await _progressFactory.InitializeProgress();
		}

		public async UniTask StartAsync(CancellationToken cancellation)
		{
			await Initialize();
#if YANDEX_CODE
			_yandexSDKController.SetStatusInitialized();
#endif
			_stateMachine.Enter<MenuState>();
		}
	}
}