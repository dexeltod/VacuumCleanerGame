using Sources.Application.StateMachine;
using Sources.Infrastructure;
using Sources.ServicesInterfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Sources.Application
{
	public class Game : IStartable
	{
		private readonly GameStateMachine _gameStateMachine;

		// [Inject]
		// public Game(GameStateMachine gameStateMachine) =>
		// 	_gameStateMachine = gameStateMachine;
		public Game(GameStateMachine assetProvider)
		{
			_gameStateMachine = assetProvider;
		}
			

		public void Start()
		{
			Debug.Log("Game is starting");

			// _gameStateMachine.Enter<InitializeServicesAndProgressState>();
		}
	}
}