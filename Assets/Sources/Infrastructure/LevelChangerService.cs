using System;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Configs.Scripts.Level;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.Services;
using Sources.InfrastructureInterfaces.States;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Advertisement;
using UnityEngine;
using VContainer;

namespace Sources.Infrastructure
{
	public sealed class LevelChangerService : ILevelChangerService
	{
		private readonly ILevelProgressFacade _levelProgressFacade;
		private readonly IGameStateChangerProvider _gameStateChangerProvider;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly IResourcesProgressPresenterProvider _progressPresenter;
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly IAdvertisement _rewardService;

		[Inject]
		public LevelChangerService(
			ILevelProgressFacade levelProgressFacade,
			IGameStateChangerProvider gameStateMachine,
			ILevelConfigGetter levelConfigGetter,
			IResourcesProgressPresenterProvider progressPresenter,
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

		private IGameStateChanger GameStateChanger => _gameStateChangerProvider.Implementation;
		private int LevelNumber => _levelProgressFacade.CurrentLevel;

		public void GoNextLevelWithReward() =>
			_rewardService.ShowAd(OnRewarded);

		private async void OnRewarded()
		{
			_levelProgressFacade.SetNextLevel();
			_progressPresenter.Implementation.ClearTotalResources();

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