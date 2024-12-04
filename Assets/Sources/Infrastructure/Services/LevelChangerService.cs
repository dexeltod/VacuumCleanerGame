using System;
using Sources.ApplicationServicesInterfaces;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Configs;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.Services;
using Sources.InfrastructureInterfaces.States;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Advertisement;
using UnityEngine;
using VContainer;

namespace Sources.Infrastructure.Services
{
	public sealed class LevelChangerService : ILevelChangerService
	{
		private readonly ILevelProgressFacade _levelProgressFacade;
		private readonly IGameStateChangerProvider _gameStateChangerProvider;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly IResourcesProgressPresenterProvider _resourcePorgressPresenterProvider;
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly IAdvertisement _rewardService;
		private readonly ILeaderBoardService _leaderBoardService;
		private readonly IPersistentProgressServiceProvider _persistentProgressServiceProvider;

		[Inject]
		public LevelChangerService(
			ILevelProgressFacade levelProgressFacade,
			IGameStateChangerProvider gameStateMachine,
			ILevelConfigGetter levelConfigGetter,
			IResourcesProgressPresenterProvider progressPresenter,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			IAdvertisement advertisement,
			ILeaderBoardService leaderBoardService,
			IPersistentProgressServiceProvider persistentProgressServiceProvider
		)
		{
			if (leaderBoardService == null) throw new ArgumentNullException(nameof(leaderBoardService));
			_levelProgressFacade = levelProgressFacade ?? throw new ArgumentNullException(nameof(levelProgressFacade));
			_gameStateChangerProvider = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));
			_resourcePorgressPresenterProvider
				= progressPresenter ?? throw new ArgumentNullException(nameof(progressPresenter));
			_progressSaveLoadDataService = progressSaveLoadDataService ??
				throw new ArgumentNullException(nameof(progressSaveLoadDataService));

			_rewardService = advertisement ?? throw new ArgumentNullException(nameof(advertisement));
			_leaderBoardService = leaderBoardService ?? throw new ArgumentNullException(nameof(leaderBoardService));
			_persistentProgressServiceProvider = persistentProgressServiceProvider ??
				throw new ArgumentNullException(nameof(persistentProgressServiceProvider));
		}

		private int LevelProgressMaxTotalResourceCount =>
			_persistentProgressServiceProvider.Self.GlobalProgress
				.LevelProgress.MaxTotalResourceCount;

		private IGameStateChanger GameStateChanger => _gameStateChangerProvider.Self;
		private int LevelNumber => _levelProgressFacade.CurrentLevel;

		public void GoNextLevelWithReward() =>
			_rewardService.ShowInterstitialAd(OnRewarded, OnRewarded, OnRewarded);

		private async void OnRewarded()
		{
			_leaderBoardService.AddScore(LevelProgressMaxTotalResourceCount);
			_levelProgressFacade.SetNextLevel();
			_resourcePorgressPresenterProvider.Self.ClearTotalResources();

			await _progressSaveLoadDataService.SaveToCloud();

			ILevelConfig levelConfig = _levelConfigGetter.GetOrDefault(LevelNumber);

			OnProcessEnded();

			GameStateChanger.Enter<IBuildSceneState, ILevelConfig>(levelConfig);
		}

		private void OnProcessEnded()
		{
			AudioListener.volume = 1;
			Time.timeScale = 1;
		}
	}
}