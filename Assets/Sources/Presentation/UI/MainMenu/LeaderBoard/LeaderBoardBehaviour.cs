using System.Collections.Generic;
using UnityEngine;

namespace Sources.Presentation.UI.MainMenu.LeaderBoard
{
	public class LeaderBoardBehaviour : MonoBehaviour
	{
		[SerializeField] private LeaderBoardPlayerPanelBehaviour _playerPanel;
		[SerializeField] private GameObject _leaderBoardContainer;

		public void InstantiatePanels(Dictionary<string, int> players)
		{
			if (_playerPanel == null)
				Debug.LogError("PANEL IS NULL");

			foreach (KeyValuePair<string, int> player in players)
			{
				LeaderBoardPlayerPanelBehaviour panel = Instantiate(_playerPanel, _leaderBoardContainer.transform);
				panel.Construct(player.Key, player.Value);
			}
		}
	}
}