using Sources.Application.StateMachine;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Sources.Application
{
	public class Game : IInitializable, IStartable
	{
		private readonly GameStateMachineFactory _gameStateMachineFactory;
		private GameStateMachine _stateMachine;

		[Inject]
		public Game(GameStateMachineFactory assetProvider)
		{
			_gameStateMachineFactory = assetProvider;
		}

		public void Start()
		{
			Debug.Log("work");
		}

		public void Initialize()
		{
			_stateMachine = _gameStateMachineFactory.Create();
		}
	}
}