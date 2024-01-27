using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sources.Application;
using Sources.ApplicationServicesInterfaces;
using Sources.Presentation.UI.MainMenu.LeaderBoard;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;
using UnityEngine;

namespace Sources.Infrastructure.Factories.LeaderBoard
{
	public class MainMenuFactory
	{
		private const int LeaderBoardPlayersCount = 5;

		private readonly IAssetProvider _assetProvider;
		private readonly ILeaderBoardService _leaderBoardService;
		private readonly ITranslatorService _translatorService;

		private LeaderBoardBehaviour _leaderBoardBehaviour;

		public MainMenuBehaviour MainMenuBehaviour { get; private set; }
		public List<string> Phrases => MainMenuBehaviour.TranslatorBehaviour.Phrases;
		private string MainMenuCanvas => ResourcesAssetPath.Scene.UIResources.MainMenuCanvas;

		public MainMenuFactory(
			IAssetProvider assetProvider,
			ILeaderBoardService leaderBoardService,
			ITranslatorService translatorService
		)
		{
			_assetProvider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));
			_leaderBoardService = leaderBoardService ?? throw new ArgumentNullException(nameof(leaderBoardService));
			_translatorService = translatorService;
		}

		public async UniTask Create()
		{
			GameObject gameObject = _assetProvider.Instantiate(MainMenuCanvas);

			_leaderBoardBehaviour = gameObject.GetComponent<LeaderBoardBehaviour>();
			MainMenuBehaviour = gameObject.GetComponent<MainMenuBehaviour>();

			MainMenuBehaviour.TranslatorBehaviour.Phrases = _translatorService.Localize(MainMenuBehaviour.TranslatorBehaviour.Phrases);
			InstantiateAndLeaders(await _leaderBoardService.GetLeaders(LeaderBoardPlayersCount));
		}

		private void InstantiateAndLeaders(Dictionary<string, int> leaders)
		{
			foreach (KeyValuePair<string, int> player in leaders)
			{
				Vector3 containerPosition = _leaderBoardBehaviour.Container.position;
				GameObject playerPanelGameObject = _leaderBoardBehaviour.PlayerPanel.gameObject;

				var a = _assetProvider
					.Instantiate(playerPanelGameObject, containerPosition)
					.GetComponent<LeaderBoardPlayerPanelBehaviour>();
				a.Construct(player.Key, player.Value);
			}
		}
	}
}