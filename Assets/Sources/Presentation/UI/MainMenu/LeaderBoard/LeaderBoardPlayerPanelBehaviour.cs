using Sources.PresentationInterfaces;
using TMPro;
using UnityEngine;

namespace Sources.Presentation.UI.MainMenu.LeaderBoard
{
	public class LeaderBoardPlayerPanelBehaviour : MonoBehaviour, ILeaderBoardPlayerPanelBehaviour
	{
		[SerializeField] private TextMeshProUGUI _playerName;
		[SerializeField] private TextMeshProUGUI _playerScore;

		public void Construct(string playerName, int playerScore)
		{
			_playerName.SetText(playerName);
			_playerScore.SetText(playerScore.ToString());
		}

		public GameObject GameObject => this.gameObject;
	}
}