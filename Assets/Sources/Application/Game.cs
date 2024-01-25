using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.Application.StateMachine;
using Sources.Application.StateMachine.GameStates;
using Sources.ApplicationServicesInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Factories;
using VContainer;
using VContainer.Unity;

namespace Sources.Application
{
	public class Game : IAsyncStartable
	{
		private readonly GameStateMachineFactory _gameStateMachineFactory;
		private readonly ProgressFactory _progressFactory;
		private readonly ISaveLoader _saveLoader;
		private readonly IYandexSDKController _yandexSDKController;
		private GameStateMachine _stateMachine;

		[Inject]
		public Game(
			GameStateMachineFactory gameStateMachineFactory,
			ProgressFactory progressFactory,
			ISaveLoader saveLoader
#if YANDEX_CODE
			, IYandexSDKController yandexSDKController
#endif
		)
		{
			_gameStateMachineFactory = gameStateMachineFactory ??
				throw new ArgumentNullException(nameof(gameStateMachineFactory));

			_progressFactory = progressFactory ?? throw new ArgumentNullException(nameof(progressFactory));
			_saveLoader = saveLoader ?? throw new ArgumentNullException(nameof(saveLoader));
#if YANDEX_CODE
			_yandexSDKController = yandexSDKController ?? throw new ArgumentNullException(nameof(yandexSDKController));
#endif
		}

		private async UniTask Initialize()
		{
			await _saveLoader.Initialize();
			await _progressFactory.InitializeProgress();
			_stateMachine = _gameStateMachineFactory.Create();
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