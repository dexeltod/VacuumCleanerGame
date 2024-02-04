using System;
using Sources.ApplicationServicesInterfaces.StateMachineInterfaces;
using Sources.DomainInterfaces;
using Sources.PresentersInterfaces;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Advertisement;
using Sources.Utils.Configs.Scripts;
using UnityEngine;

namespace Sources.Controllers
{
	public class LevelChangerPresenter : IDisposable, ILevelChangerPresenter
	{
		private readonly ILevelProgressFacade _levelProgressFacade;
		private readonly IGameStateMachine _gameStateMachine;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly IResourcesProgressPresenter _progressPresenter;
		private readonly IProgressLoadDataService _progressLoadDataService;
		private readonly IAdvertisement _rewardService;

		private IGoToTextLevelButtonObserver _button;

		private int LevelNumber => _levelProgressFacade.CurrentLevelNumber;

		public LevelChangerPresenter(
			ILevelProgressFacade levelProgressFacade,
			IGameStateMachine gameStateMachine,
			ILevelConfigGetter levelConfigGetter,
			IResourcesProgressPresenter progressPresenter,
			IProgressLoadDataService progressLoadDataService,
			IAdvertisement advertisement
		)
		{
			_levelProgressFacade = levelProgressFacade ?? throw new ArgumentNullException(nameof(levelProgressFacade));
			_gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));
			_progressPresenter = progressPresenter ?? throw new ArgumentNullException(nameof(progressPresenter));
			_progressLoadDataService = progressLoadDataService ??
				throw new ArgumentNullException(nameof(progressLoadDataService));

			_rewardService = advertisement ?? throw new ArgumentNullException(nameof(advertisement));
		}

		public void SetButton(IGoToTextLevelButtonObserver button) =>
			_button = button ?? throw new ArgumentNullException(nameof(button));

		public void Dispose() =>
			_button.GoToTextLevelButtonClicked -= OnGoToTextLevelButtonClicked;

		public void Enable()
		{
			_button.GoToTextLevelButtonClicked += OnGoToTextLevelButtonClicked;
			_button.ButtonDestroying += OnButtonDestroying;
		}

		public void Disable()
		{
			_button.GoToTextLevelButtonClicked += OnGoToTextLevelButtonClicked;
			_button.ButtonDestroying += OnButtonDestroying;
		}

		private void OnButtonDestroying() =>
			_button.GoToTextLevelButtonClicked -= OnGoToTextLevelButtonClicked;

		private void OnGoToTextLevelButtonClicked() =>
			_rewardService.ShowAd(OnAdShowed, OnRewarded, OnProcessEnded);

		private void OnAdShowed()
		{
			AudioListener.volume = 0;
			Time.timeScale = 0;
		}

		private void OnProcessEnded()
		{
			AudioListener.volume = 1;
			Time.timeScale = 1;
		}

		private async void OnRewarded()
		{
			_levelProgressFacade.SetNextLevel();
			_progressPresenter.ClearScores();
			await _progressLoadDataService.SaveToCloud();

			LevelConfig levelConfig = _levelConfigGetter.Get(LevelNumber);

			OnProcessEnded();

			_gameStateMachine.Enter<BuildSandState, LevelConfig>(levelConfig);
		}
	}
}