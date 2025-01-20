using System;
using Sources.BusinessLogic.Interfaces;
using Sources.BusinessLogic.Interfaces.Factory;
using Sources.BusinessLogic.Services;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Controllers.MainMenu;
using Sources.ControllersInterfaces;
using Sources.Domain.Settings;
using Sources.DomainInterfaces;
using Sources.PresentationInterfaces;
using Sources.Utils;
using UnityEngine;
using UnityEngine.Audio;

namespace Sources.Boot.Scripts.Factories.Presenters
{
	public class MainMenuPresenterFactory : IMainMenuPresenterFactory
	{
		private readonly IAssetFactory _assetFactory;
		private readonly IMainMenuView _mainMenu;
		private readonly ILevelProgressFacade _levelProgress;
		private readonly IGameStateChanger _stateMachine;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly IAuthorizationPresenter _authorizationPresenter;
		private readonly ILeaderBoardView _leaderBoardView;
		private readonly ILeaderBoardService _leaderBoardService;
		private readonly ILeaderBoardPlayersFactory _leaderBoardPlayersFactory;
		private readonly ISettingsView _settingsView;

		public MainMenuPresenterFactory(IAssetFactory assetFactory,
			IMainMenuView mainMenu,
			ILevelProgressFacade levelProgress,
			IGameStateChanger stateMachine,
			ILevelConfigGetter levelConfigGetter,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			IAuthorizationPresenter authorizationPresenter,
			ILeaderBoardView leaderBoardView,
			ILeaderBoardService leaderBoardService,
			ILeaderBoardPlayersFactory leaderBoardPlayersFactory,
			ISettingsView settingsView)
		{
			if (assetFactory == null) throw new ArgumentNullException(nameof(assetFactory));

			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_mainMenu = mainMenu ?? throw new ArgumentNullException(nameof(mainMenu));
			_levelProgress = levelProgress ?? throw new ArgumentNullException(nameof(levelProgress));
			_stateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));
			_progressSaveLoadDataService = progressSaveLoadDataService ??
			                               throw new ArgumentNullException(nameof(progressSaveLoadDataService));
			_authorizationPresenter = authorizationPresenter ?? throw new ArgumentNullException(nameof(authorizationPresenter));
			_leaderBoardView = leaderBoardView ?? throw new ArgumentNullException(nameof(leaderBoardView));
			_leaderBoardService = leaderBoardService ?? throw new ArgumentNullException(nameof(leaderBoardService));
			_leaderBoardPlayersFactory =
				leaderBoardPlayersFactory ?? throw new ArgumentNullException(nameof(leaderBoardPlayersFactory));
			_settingsView = settingsView ?? throw new ArgumentNullException(nameof(settingsView));
		}

		public IMainMenuPresenter Create()
		{
			var mixer = _assetFactory.LoadFromResources<AudioMixer>(ResourcesAssetPath.GameObjects.AudioMixer);

			var soundSettings = new SoundSettings(PlayerPrefs.GetFloat(SettingsPlayerPrefsNames.MasterVolumeName));
			return new MainMenuPresenter(
				_mainMenu,
				_levelProgress,
				_stateMachine,
				_levelConfigGetter,
				_progressSaveLoadDataService,
				_authorizationPresenter,
				_leaderBoardView,
				_leaderBoardService,
				_settingsView,
				mixer,
				_leaderBoardPlayersFactory,
				soundSettings
			);
		}
	}
}
