using System;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces.Presenters;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.Services;
using Sources.InfrastructureInterfaces.States;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Advertisement;
using Sources.Utils.Configs.Scripts;
using UnityEngine;
using VContainer;

namespace Sources.Infrastructure
{
	public sealed class LevelChangerService : ILevelChangerService
	{
		private readonly ILevelProgressFacade _levelProgressFacade;
		private readonly IGameStateChangerProvider _gameStateChangerProvider;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly IResourcesProgressPresenter _progressPresenter;
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly IAdvertisement _rewardService;

		private IGameStateChanger GameStateChanger => _gameStateChangerProvider.Instance;

		private int LevelNumber => _levelProgressFacade.CurrentLevelNumber;

		[Inject]
		public LevelChangerService(
			ILevelProgressFacade levelProgressFacade,
			IGameStateChangerProvider gameStateMachine,
			ILevelConfigGetter levelConfigGetter,
			IResourcesProgressPresenter progressPresenter,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			IAdvertisement advertisement
		)
		{
			_levelProgressFacade = levelProgressFacade ?? throw new ArgumentNullException(nameof(levelProgressFacade));
			_gameStateChangerProvider = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));
			_progressPresenter = progressPresenter ?? throw new ArgumentNullException(nameof(progressPresenter));
			_progressSaveLoadDataService = progressSaveLoadDataService ??
				throw new ArgumentNullException(nameof(progressSaveLoadDataService));

			_rewardService = advertisement ?? throw new ArgumentNullException(nameof(advertisement));
		}

		public void GoNextLevelWithReward() =>
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

			await _progressSaveLoadDataService.SaveToCloud();

			LevelConfig levelConfig = _levelConfigGetter.Get(LevelNumber);

			OnProcessEnded();

			GameStateChanger.Enter<IBuildSceneState, LevelConfig>(levelConfig);
		}
	}
}