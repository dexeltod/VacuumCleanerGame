using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sources.ApplicationServicesInterfaces;
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

		public MainMenuBehaviour MainMenuBehaviour { get; private set; }
		private List<string> Phrases => MainMenuBehaviour.Translator.Phrases;
		private string MainMenuCanvas => ResourcesAssetPath.Scene.UIResources.MainMenuCanvas;

		public async UniTask Create()
		{
			GameObject gameObject = _assetFactory.Instantiate(MainMenuCanvas);

			var leaderBoardBehaviour = gameObject.GetComponent<LeaderBoardBehaviour>();
			var leaderBoardFactory = new LeaderBoardFactory(_assetFactory, leaderBoardBehaviour);

			MainMenuBehaviour = gameObject.GetComponent<MainMenuBehaviour>();

			MainMenuBehaviour.Translator.Phrases = _translatorService.Localize(MainMenuBehaviour.Translator.Phrases);

			leaderBoardFactory.Create(await _leaderBoardService.GetLeaders(LeaderBoardPlayersCount));
		}
	}
}