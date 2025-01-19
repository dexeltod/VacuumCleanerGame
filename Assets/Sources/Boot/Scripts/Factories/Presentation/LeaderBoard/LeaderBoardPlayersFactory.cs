using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sources.BusinessLogic;
using Sources.BusinessLogic.Interfaces;
using Sources.BusinessLogic.Interfaces.Factory;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Presentation.UI.MainMenu.LeaderBoard;
using Sources.PresentationInterfaces;
using UnityEngine;
using VContainer;

namespace Sources.Boot.Scripts.Factories.Presentation.LeaderBoard
{
	public class LeaderBoardPlayersFactory : ILeaderBoardPlayersFactory
	{
		private const int LeaderBoardPlayersCount = 5;
		private const string AnonymousPlayerName = "Anonymous";

		private readonly IAssetFactory _assetFactory;
		private readonly ILeaderBoardService _leaderBoardService;
		private readonly TranslatorService _translatorService;

		[Inject]
		public LeaderBoardPlayersFactory(
			IAssetFactory assetFactory,
			ILeaderBoardService leaderBoardService,
			TranslatorService translatorService
		)
		{
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_leaderBoardService = leaderBoardService ?? throw new ArgumentNullException(nameof(leaderBoardService));
			_translatorService = translatorService;
		}

		public async UniTask Create(ILeaderBoardView leaderBoardView)
		{
#if YANDEX_CODE
			Dictionary<string, int> leaders = await _leaderBoardService.GetLeaders(LeaderBoardPlayersCount);
#endif
#if !YANDEX_CODE
			Dictionary<string, int> leaders = GetTestLeads();
#endif

			if (leaders == null) throw new ArgumentNullException(nameof(leaders));

			List<KeyValuePair<string, int>> sortedLeaders = leaders.ToList();

			sortedLeaders.Sort((x, y) => y.Value.CompareTo(x.Value));

			foreach (var player in sortedLeaders)
			{
				string name = player.Key;
				int score = player.Value;

				Transform containerTransform = leaderBoardView.Container;
				GameObject playerPanelGameObject = leaderBoardView.PlayerPanel.GameObject;

				LeaderBoardPlayerPanelBehaviour panel = _assetFactory.Instantiate(
					playerPanelGameObject,
					containerTransform
				).GetComponent<LeaderBoardPlayerPanelBehaviour>();

				if (string.IsNullOrWhiteSpace(name))
				{
					panel.Construct(_translatorService.GetLocalize(AnonymousPlayerName), score);
					return;
				}

				panel.Construct(player.Key, player.Value);
			}
		}

		private Dictionary<string, int> GetTestLeads() =>
			new()
			{
				{ "Player1", 100 },
				{ "Player2", 200 },
				{ "Player3", 300 },
				{ "Player4", 400 },
				{ "Player5", 500 }
			};
	}
}