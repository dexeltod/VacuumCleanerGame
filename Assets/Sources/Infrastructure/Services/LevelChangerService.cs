using System;
using Sources.BusinessLogic.Interfaces;
using Sources.BusinessLogic.Services;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.BusinessLogic.ServicesInterfaces.Advertisement;
using Sources.BusinessLogic.States;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using UnityEngine;

namespace Sources.Infrastructure.Services
{
	public sealed class LevelChangerService : ILevelChangerService
	{
		private readonly ILeaderBoardService _leaderBoardService;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly ILevelProgressFacade _levelProgressFacade;
		private readonly IPersistentProgressService _persistentProgressService;
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly IResourcesProgressPresenter _resourcesProgressPresenter;
		private readonly IAdvertisement _rewardService;
		private readonly IStateMachine _stateMachine;

		public LevelChangerService(
			ILevelProgressFacade levelProgressFacade,
			IStateMachine stateMachine,
			ILevelConfigGetter levelConfigGetter,
			IResourcesProgressPresenter progressPresenter,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			IAdvertisement advertisement,
			ILeaderBoardService leaderBoardService,
			IPersistentProgressService persistentProgressService
		)
		{
			if (leaderBoardService == null) throw new ArgumentNullException(nameof(leaderBoardService));

			_levelProgressFacade = levelProgressFacade ?? throw new ArgumentNullException(nameof(levelProgressFacade));
			_stateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
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
			_persistentProgressService.GlobalProgress.LevelProgress.MaxTotalResourceCount;

		private int LevelNumber => _levelProgressFacade.CurrentLevel;

		public void GoNextLevelWithReward() =>
			_rewardService.ShowInterstitialAd(OnRewarded, OnRewarded, OnRewarded);

		private void OnProcessEnded()
		{
			AudioListener.volume = 1;
			Time.timeScale = 1;
		}

		private async void OnRewarded()
		{
			_leaderBoardService.AddScore(LevelProgressMaxTotalResourceCount);
			_levelProgressFacade.SetNextLevel();
			_resourcesProgressPresenter.ClearTotalResources();

			await _progressSaveLoadDataService.SaveToCloud();

			ILevelConfig levelConfig = _levelConfigGetter.GetOrDefault(LevelNumber);

			OnProcessEnded();

			_stateMachine.Enter<IBuildSceneState, ILevelConfig>(levelConfig);
		}
	}
}
