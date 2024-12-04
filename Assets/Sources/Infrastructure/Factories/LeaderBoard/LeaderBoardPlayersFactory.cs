using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sources.ApplicationServicesInterfaces;
using Sources.InfrastructureInterfaces.Factory;
using Sources.Presentation.UI.MainMenu.LeaderBoard;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Infrastructure.Factories.LeaderBoard
{
	public class LeaderBoardPlayersFactory : ILeaderBoardPlayersFactory
	{
		private const int LeaderBoardPlayersCount = 5;
		private const string AnonymousPlayerName = "Anonymous";

		private readonly IAssetFactory _assetFactory;
		private readonly ILeaderBoardService _leaderBoardService;
		private readonly ITranslatorService _translatorService;
		private readonly LeaderBoardView _leaderBoardView;

		public LeaderBoardPlayersFactory(
			IAssetFactory assetFactory,
			LeaderBoardView leaderBoardView,
			ILeaderBoardService leaderBoardService,
			ITranslatorService translatorService
		)
		{
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_leaderBoardService = leaderBoardService ?? throw new ArgumentNullException(nameof(leaderBoardService));
			_translatorService = translatorService;
			_leaderBoardView = leaderBoardView ? leaderBoardView : throw new ArgumentNullException(nameof(leaderBoardView));
		}

		public async UniTask Create()
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

				Transform containerTransform = _leaderBoardView.Container;
				GameObject playerPanelGameObject = _leaderBoardView.PlayerPanel.GameObject;

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