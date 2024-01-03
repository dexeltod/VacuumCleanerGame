using System;
using Sources.Application.StateMachine.GameStates;
using Sources.ApplicationServicesInterfaces;
using Sources.ApplicationServicesInterfaces.StateMachineInterfaces;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Scene;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;
using UnityEngine;

namespace Sources.Infrastructure.Presenters
{
	public class LevelChangerPresenter : IDisposable
	{
		private readonly ILevelProgressFacade _levelProgressFacade;
		private readonly IGameStateMachine _gameStateMachine;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly IResourcesProgressPresenter _progressPresenter;
		private readonly IProgressLoadDataService _progressLoadDataService;
		private readonly IYandexSDKController _yandexSDKController;

		private IGoToTextLevelButtonSubscribeable _button;

		public LevelChangerPresenter(
			ILevelProgressFacade levelProgressFacade,
			IGameStateMachine gameStateMachine,
			ILevelConfigGetter levelConfigGetter,
			IResourcesProgressPresenter progressPresenter,
			IProgressLoadDataService progressLoadDataService,
			IYandexSDKController yandexSDKController
		)
		{
			_levelProgressFacade = levelProgressFacade ?? throw new ArgumentNullException(nameof(levelProgressFacade));
			_gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));
			_progressPresenter = progressPresenter ?? throw new ArgumentNullException(nameof(progressPresenter));
			_progressLoadDataService = progressLoadDataService ??
				throw new ArgumentNullException(nameof(progressLoadDataService));

			_yandexSDKController = yandexSDKController ?? throw new ArgumentNullException(nameof(yandexSDKController));
		}

		public void Dispose() =>
			_button.GoToTextLevelButtonClicked -= OnGoToTextLevelButtonClicked;

		public void SetButton(IGoToTextLevelButtonSubscribeable button)
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
			await _yandexSDKController.ShowAd(
				OnAdShowed,
				OnRewarded,
				OnAdClosed
			);
		}

		private void OnAdShowed()
		{
			AudioListener.volume = 0;
			Time.timeScale = 0;
		}

		private void OnAdClosed()
		{
			AudioListener.volume = 1;
			Time.timeScale = 1;
		}

		private async void OnRewarded()
		{
			_levelProgressFacade.SetNextLevel();
			_progressPresenter.ClearScores();
			await _progressLoadDataService.SaveToCloud();

			LevelConfig levelConfig = _levelConfigGetter.Get(_levelProgressFacade.CurrentLevelNumber);

			AudioListener.volume = 1;
			Time.timeScale = 1;

			_gameStateMachine.Enter<BuildSandState, LevelConfig>(levelConfig);
		}
	}
}