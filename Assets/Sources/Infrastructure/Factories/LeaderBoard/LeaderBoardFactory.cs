using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Presentation.UI.MainMenu.LeaderBoard;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Infrastructure.Factories.LeaderBoard
{
	public class LeaderBoardFactory
	{
		private readonly IAssetFactory _assetFactory;
		private readonly LeaderBoardView _leaderBoardView;

		public LeaderBoardFactory(
			IAssetFactory assetFactory,
			LeaderBoardView leaderBoardView
		)
		{
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_leaderBoardView = leaderBoardView
				? leaderBoardView
				: throw new ArgumentNullException(nameof(leaderBoardView));
		}

		public void Create(Dictionary<string, int> leaders)
		{
			if (leaders == null) throw new ArgumentNullException(nameof(leaders));

			List<KeyValuePair<string, int>> sortedLeaders = leaders.ToList();
			sortedLeaders.Sort((x, y) => y.Value.CompareTo(x.Value));

			foreach (KeyValuePair<string, int> player in sortedLeaders)
			{
				Transform containerTransform = _leaderBoardView.Container;
				GameObject playerPanelGameObject = _leaderBoardView.PlayerPanel.gameObject;

				var panel = _assetFactory
					.Instantiate(playerPanelGameObject)
					.GetComponent<LeaderBoardPlayerPanelBehaviour>();

				panel.transform.SetParent(containerTransform);
				panel.Construct(player.Key, player.Value);
			}
		}
	}
}