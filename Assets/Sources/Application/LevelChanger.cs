using System;
using Sources.Application.StateMachine.GameStates;
using Sources.Application.StateMachineInterfaces;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Scene;
using Sources.PresentationInterfaces;

namespace Sources.Application
{
	public class LevelChanger : IDisposable
	{
		private readonly IGoToTextLevelButtonSubscribeable _buttonSubscribeable;
		private readonly ILevelProgressPresenter           _progressPresenter;
		private readonly IGameStateMachine                 _gameStateMachine;
		private readonly ILevelConfigGetter                _levelConfigGetter;

		public LevelChanger
		(
			IGoToTextLevelButtonSubscribeable buttonSubscribeable,
			ILevelProgressPresenter           progressPresenter,
			IGameStateMachine                 gameStateMachine,
			ILevelConfigGetter levelConfigGetter
		)
		{
			_buttonSubscribeable                            =  buttonSubscribeable;
			_progressPresenter                              =  progressPresenter;
			_gameStateMachine                               =  gameStateMachine;
			_levelConfigGetter                         =  levelConfigGetter;
			_buttonSubscribeable.GoToTextLevelButtonClicked += OnGoToNextLevel;
		}

		public void Dispose() =>
			_buttonSubscribeable.GoToTextLevelButtonClicked -= OnGoToNextLevel;

		private void OnGoToNextLevel()
		{
			// _levelConfigGetter.Get()
			// _gameStateMachine.Enter<BuildSceneState, >();
		}
	}
}