using System;
using JetBrains.Annotations;
using Sources.Application.StateMachine.GameStates;
using Sources.Application.StateMachineInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Scene;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs;

namespace Sources.Application
{
	public class LevelChangerPresenter : IDisposable
	{
		private readonly ILevelProgressFacade _levelProgressFacade;
		private readonly IGameStateMachine _gameStateMachine;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly IResourcesProgressPresenter _progressPresenter;
		private readonly IProgressLoadDataService _progressLoadDataService;

		private IGoToTextLevelButtonSubscribeable _button;

		public LevelChangerPresenter
		(
			[NotNull] ILevelProgressFacade levelProgressFacade,
			[NotNull] IGameStateMachine gameStateMachine,
			[NotNull] ILevelConfigGetter levelConfigGetter,
			[NotNull] IResourcesProgressPresenter progressPresenter,
			[NotNull] IProgressLoadDataService progressLoadDataService
		)
		{
			_levelProgressFacade = levelProgressFacade ?? throw new ArgumentNullException(nameof(levelProgressFacade));
			_gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));
			_progressPresenter = progressPresenter ?? throw new ArgumentNullException(nameof(progressPresenter));

			_progressLoadDataService = progressLoadDataService ??
				throw new ArgumentNullException(nameof(progressLoadDataService));
		}

		public void SetAction(IGoToTextLevelButtonSubscribeable button)
		{
			_button = button ?? throw new ArgumentNullException(nameof(button));

			_button.GoToTextLevelButtonClicked += OnGoToTextLevelButtonClicked;
			_button.ButtonDestroying += OnButtonDestroying;
		}

		private void OnButtonDestroying()
		{
			_button.GoToTextLevelButtonClicked -= OnGoToTextLevelButtonClicked;
			_button.GoToTextLevelButtonClicked -= OnGoToTextLevelButtonClicked;
		}

		private async void OnGoToTextLevelButtonClicked()
		{
			_levelProgressFacade.SetNextLevel();
			_progressPresenter.ClearScores();

			await _progressLoadDataService.SaveToCloud();

			LevelConfig levelConfig = _levelConfigGetter.Get(_levelProgressFacade.CurrentLevelNumber);

			_gameStateMachine.Enter<BuildSandState, LevelConfig>(levelConfig);
		}

		public void Dispose() =>
			_button.GoToTextLevelButtonClicked -= OnGoToTextLevelButtonClicked;
	}
}