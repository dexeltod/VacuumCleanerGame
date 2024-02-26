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
		private readonly LeaderBoardBehaviour _leaderBoardBehaviour;

		public LeaderBoardFactory(
			IAssetFactory assetFactory,
			LeaderBoardBehaviour leaderBoardBehaviour
		)
		{
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_leaderBoardBehaviour = leaderBoardBehaviour
				? leaderBoardBehaviour
				: throw new ArgumentNullException(nameof(leaderBoardBehaviour));
		}

		public void Create(Dictionary<string, int> leaders)
		{
			if (leaders == null) throw new ArgumentNullException(nameof(leaders));

			List<KeyValuePair<string, int>> sortedLeaders = leaders.ToList();
			sortedLeaders.Sort((x, y) => y.Value.CompareTo(x.Value));

			foreach (KeyValuePair<string, int> player in sortedLeaders)
			{
				Transform containerTransform = _leaderBoardBehaviour.Container;
				GameObject playerPanelGameObject = _leaderBoardBehaviour.PlayerPanel.gameObject;

				var panel = _assetFactory
					.Instantiate(playerPanelGameObject)
					.GetComponent<LeaderBoardPlayerPanelBehaviour>();

				panel.transform.SetParent(containerTransform);
				panel.Construct(player.Key, player.Value);
			}
		}
	}
}