using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sources.ApplicationServicesInterfaces;
using Sources.Presentation;
using Sources.Presentation.UI;
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

		private readonly IAssetFactory _assetFactory;
		private readonly ILeaderBoardService _leaderBoardService;
		private readonly ITranslatorService _translatorService;

		private LeaderBoardBehaviour _leaderBoardBehaviour;

		public MainMenuBehaviour MainMenuBehaviour { get; private set; }
		private List<string> Phrases => MainMenuBehaviour.Translator.Phrases;
		private string MainMenuCanvas => ResourcesAssetPath.Scene.UIResources.MainMenuCanvas;

		public MainMenuFactory(
			IAssetFactory assetFactory,
			ILeaderBoardService leaderBoardService,
			ITranslatorService translatorService
		)
		{
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_leaderBoardService = leaderBoardService ?? throw new ArgumentNullException(nameof(leaderBoardService));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
		}

		public async UniTask Create()
		{
			GameObject gameObject = _assetFactory.Instantiate(MainMenuCanvas);

			_leaderBoardBehaviour = gameObject.GetComponent<LeaderBoardBehaviour>();
			MainMenuBehaviour = gameObject.GetComponent<MainMenuBehaviour>();

			MainMenuBehaviour.Translator.Phrases = _translatorService.Localize(MainMenuBehaviour.Translator.Phrases);
			InstantiateAndLeaders(await _leaderBoardService.GetLeaders(LeaderBoardPlayersCount));
		}

		private void InstantiateAndLeaders(Dictionary<string, int> leaders)
		{
			foreach (KeyValuePair<string, int> player in leaders)
			{
				Vector3 containerPosition = _leaderBoardBehaviour.Container.position;
				GameObject playerPanelGameObject = _leaderBoardBehaviour.PlayerPanel.gameObject;

				var panel = _assetFactory
					.Instantiate(playerPanelGameObject, containerPosition)
					.GetComponent<LeaderBoardPlayerPanelBehaviour>();
				panel.Construct(player.Key, player.Value);
			}
		}
	}
}