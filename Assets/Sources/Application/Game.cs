using Sources.Application.StateMachine;
using Sources.Application.StateMachine.GameStates;
using Sources.Infrastructure.Factories;
using VContainer;
using VContainer.Unity;

namespace Sources.Application
{
	public class Game : IInitializable, IStartable
	{
		private readonly GameStateMachineFactory _gameStateMachineFactory;
		private readonly ProgressFactory _progressFactory;
		private GameStateMachine _stateMachine;

		[Inject]
		public Game(GameStateMachineFactory assetProvider, ProgressFactory progressFactory)
		{
			_gameStateMachineFactory = assetProvider;
			_progressFactory = progressFactory;
		}

		public async void Start()
		{
			_stateMachine.Enter<MenuState>();
		}

		public async void Initialize()
		{
			await _progressFactory.InitializeProgress();
			_stateMachine = _gameStateMachineFactory.Create();
		}
	}
}