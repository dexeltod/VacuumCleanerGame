using System;
using Sources.BusinessLogic.Interfaces;
using Sources.BusinessLogic.Interfaces.Factory;
using Sources.BusinessLogic.Services;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Controllers.MainMenu;
using Sources.ControllersInterfaces;
using Sources.ControllersInterfaces.Factories;
using Sources.Domain.Settings;
using Sources.DomainInterfaces;
using Sources.PresentationInterfaces;
using Sources.Utils;
using UnityEngine;
using UnityEngine.Audio;

namespace Sources.Controllers.Factories
{
	public class MainMenuPresenterFactory : IMainMenuPresenterFactory
	{
		private readonly IAssetFactory _assetFactory;

		public MainMenuPresenterFactory(IAssetFactory assetFactory)
		{
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
		}

		public MainMenuPresenter Create(IMainMenuView mainMenu,
			ILevelProgressFacade levelProgress,
			IGameStateChanger stateMachine,
			ILevelConfigGetter levelConfigGetter,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			IAuthorizationPresenter authorizationPresenter,
			ILeaderBoardView leaderBoardView,
			ILeaderBoardService leaderBoardService,
			ILeaderBoardPlayersFactory leaderBoardPlayersFactory)
		{
			return new MainMenuPresenter(
				mainMenu,
				levelProgress,
				stateMachine,
				levelConfigGetter,
				progressSaveLoadDataService,
				authorizationPresenter,
				leaderBoardView,
				leaderBoardService,
				mainMenu.GetSettingsView(),
				_assetFactory.LoadFromResources<AudioMixer>(ResourcesAssetPath.GameObjects.AudioMixer),
				leaderBoardPlayersFactory,
				new SoundSettings(PlayerPrefs.GetFloat(SettingsPlayerPrefsNames.MasterVolumeName))
			);
		};
	}
}
