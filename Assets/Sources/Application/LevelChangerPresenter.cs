using System;
using Sources.Application.StateMachine.GameStates;
using Sources.Application.StateMachineInterfaces;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Scene;
using Sources.PresentationInterfaces;
using Sources.Utils.Configs;

namespace Sources.Application
{
	public class LevelChangerPresenter : IDisposable
	{
		private readonly ILevelProgressFacade _levelProgressFacade;
		private readonly IGameStateMachine    _gameStateMachine;
		private readonly ILevelConfigGetter   _levelConfigGetter;

		private IGoToTextLevelButtonSubscribeable _button;

		public LevelChangerPresenter
		(
			ILevelProgressFacade              levelProgressFacade,
			IGameStateMachine                 gameStateMachine,
			ILevelConfigGetter                levelConfigGetter
		)
		{
			_levelProgressFacade = levelProgressFacade;
			_gameStateMachine    = gameStateMachine;
			_levelConfigGetter   = levelConfigGetter;
		}

		public void SetAction(IGoToTextLevelButtonSubscribeable button)
		{
			_button = button;

			_button.GoToTextLevelButtonClicked += OnGoToTextLevelButtonClicked;
			_button.ButtonDestroying           += OnButtonDestroying;
		}

		private void OnButtonDestroying()
		{
			_button.GoToTextLevelButtonClicked -= OnGoToTextLevelButtonClicked;
			_button.GoToTextLevelButtonClicked -= OnGoToTextLevelButtonClicked;
		}

		private void OnGoToTextLevelButtonClicked()
		{
			_levelProgressFacade.SetNextLevel();
			LevelConfig levelConfig = _levelConfigGetter.Get(_levelProgressFacade.CurrentLevelNumber);

			_gameStateMachine.Enter<BuildSandState, LevelConfig>(levelConfig);
		}

		public void Dispose() =>
			_button.GoToTextLevelButtonClicked -= OnGoToTextLevelButtonClicked;
	}
}