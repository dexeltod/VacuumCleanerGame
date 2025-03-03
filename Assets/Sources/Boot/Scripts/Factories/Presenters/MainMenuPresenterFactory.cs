using System;
using Sources.BusinessLogic.Interfaces;
using Sources.BusinessLogic.Interfaces.Factory;
using Sources.BusinessLogic.Services;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Controllers.MainMenu;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.Entities;
using Sources.PresentationInterfaces;
using Sources.Utils;
using UnityEngine.Audio;

namespace Sources.Boot.Scripts.Factories.Presenters
{
	public class MainMenuPresenterFactory : IMainMenuPresenterFactory
	{
		private readonly IAssetLoader _assetLoader;
		private readonly IAuthorizationPresenter _authorizationPresenter;
		private readonly ILeaderBoardPlayersFactory _leaderBoardPlayersFactory;
		private readonly ILeaderBoardService _leaderBoardService;
		private readonly ILeaderBoardView _leaderBoardView;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly ILevelProgressFacade _levelProgress;
		private readonly IMainMenuView _mainMenu;
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly ISettingsView _settingsView;
		private readonly ISoundSettings _soundSettings;
		private readonly IStateMachine _stateMachine;

		public MainMenuPresenterFactory(
			IAssetLoader assetLoader,
			IMainMenuView mainMenu,
			ILevelProgressFacade levelProgress,
			IStateMachine stateMachine,
			ILevelConfigGetter levelConfigGetter,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			IAuthorizationPresenter authorizationPresenter,
			ILeaderBoardView leaderBoardView,
			ILeaderBoardService leaderBoardService,
			ILeaderBoardPlayersFactory leaderBoardPlayersFactory,
			ISettingsView settingsView,
			ISoundSettings soundSettings)
		{
			if (assetLoader == null) throw new ArgumentNullException(nameof(assetLoader));
			if (soundSettings == null) throw new ArgumentNullException(nameof(soundSettings));

			_assetLoader = assetLoader ?? throw new ArgumentNullException(nameof(assetLoader));
			_mainMenu = mainMenu ?? throw new ArgumentNullException(nameof(mainMenu));
			_levelProgress = levelProgress ?? throw new ArgumentNullException(nameof(levelProgress));
			_stateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));
			_progressSaveLoadDataService = progressSaveLoadDataService
			                               ?? throw new ArgumentNullException(nameof(progressSaveLoadDataService));
			_authorizationPresenter = authorizationPresenter ?? throw new ArgumentNullException(nameof(authorizationPresenter));
			_leaderBoardView = leaderBoardView ?? throw new ArgumentNullException(nameof(leaderBoardView));
			_leaderBoardService = leaderBoardService ?? throw new ArgumentNullException(nameof(leaderBoardService));
			_leaderBoardPlayersFactory =
				leaderBoardPlayersFactory ?? throw new ArgumentNullException(nameof(leaderBoardPlayersFactory));
			_settingsView = settingsView ?? throw new ArgumentNullException(nameof(settingsView));
			_soundSettings = soundSettings ?? throw new ArgumentNullException(nameof(soundSettings));
		}

		public IMainMenuPresenter Create()
		{
			var mixer = _assetLoader.LoadFromResources<AudioMixer>(ResourcesAssetPath.GameObjects.AudioMixer);

			return new MainMenuPresenter(
				_stateMachine,
				_mainMenu,
				_levelProgress,
				_levelConfigGetter,
				_progressSaveLoadDataService,
				_authorizationPresenter,
				_leaderBoardView,
				_leaderBoardService,
				_settingsView,
				mixer,
				_leaderBoardPlayersFactory,
				_soundSettings
			);
		}
	}
}