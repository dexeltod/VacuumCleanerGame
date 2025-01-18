using System;
using Sources.BusinessLogic.Interfaces;
using Sources.BusinessLogic.Services;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.BusinessLogic.ServicesInterfaces.Advertisement;
using Sources.BusinessLogic.States;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using UnityEngine;
using VContainer;

namespace Sources.Infrastructure.Services
{
	public sealed class LevelChangerService : ILevelChangerService
	{
		private readonly ILevelProgressFacade _levelProgressFacade;
		private readonly IGameStateChanger _gameStateChanger;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly IResourcesProgressPresenter _resourcesProgressPresenter;
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly IAdvertisement _rewardService;
		private readonly ILeaderBoardService _leaderBoardService;
		private readonly IPersistentProgressService _persistentProgressService;

		[Inject]
		public LevelChangerService(
			ILevelProgressFacade levelProgressFacade,
			IGameStateChanger gameStateMachine,
			ILevelConfigGetter levelConfigGetter,
			IResourcesProgressPresenter progressPresenter,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			IAdvertisement advertisement,
			ILeaderBoardService leaderBoardService,
			IPersistentProgressService persistentProgressService
		)
		{
			if (leaderBoardService == null)
				throw new ArgumentNullException(
					nameof(leaderBoardService)
				);

			_levelProgressFacade = levelProgressFacade ?? throw new ArgumentNullException(nameof(levelProgressFacade));
			_gameStateChanger = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));
			_resourcesProgressPresenter = progressPresenter ?? throw new ArgumentNullException(nameof(progressPresenter));
			_progressSaveLoadDataService = progressSaveLoadDataService ??
			                               throw new ArgumentNullException(nameof(progressSaveLoadDataService));

			_rewardService = advertisement ?? throw new ArgumentNullException(nameof(advertisement));
			_leaderBoardService = leaderBoardService ?? throw new ArgumentNullException(nameof(leaderBoardService));
			_persistentProgressService =
				persistentProgressService ?? throw new ArgumentNullException(nameof(persistentProgressService));
		}

		private int LevelProgressMaxTotalResourceCount =>
			_persistentProgressService.GlobalProgress
				.LevelProgress.MaxTotalResourceCount;

		private int LevelNumber => _levelProgressFacade.CurrentLevel;

		public void GoNextLevelWithReward() =>
			_rewardService.ShowInterstitialAd(
				OnRewarded,
				OnRewarded,
				OnRewarded
			);

		private async void OnRewarded()
		{
			_leaderBoardService.AddScore(
				LevelProgressMaxTotalResourceCount
			);
			_levelProgressFacade.SetNextLevel();
			_resourcesProgressPresenter.ClearTotalResources();

			await _progressSaveLoadDataService.SaveToCloud();

			ILevelConfig levelConfig = _levelConfigGetter.GetOrDefault(
				LevelNumber
			);

			OnProcessEnded();

			_gameStateChanger.Enter<IBuildSceneState, ILevelConfig>(
				levelConfig
			);
		}

		private void OnProcessEnded()
		{
			AudioListener.volume = 1;
			Time.timeScale = 1;
		}
	}
}
