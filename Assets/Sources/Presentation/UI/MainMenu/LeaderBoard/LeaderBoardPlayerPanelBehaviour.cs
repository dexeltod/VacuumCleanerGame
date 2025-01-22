using Sources.PresentationInterfaces;
using TMPro;
using UnityEngine;

namespace Sources.Presentation.UI.MainMenu.LeaderBoard
{
	public class LeaderBoardPlayerPanelBehaviour : MonoBehaviour, ILeaderBoardPlayerPanelBehaviour
	{
		[SerializeField] private TextMeshProUGUI _playerName;
		[SerializeField] private TextMeshProUGUI _playerScore;

		public GameObject GameObject => gameObject;

		public void Construct(string playerName, int playerScore)
		{
			_playerName.SetText(playerName);
			_playerScore.SetText(playerScore.ToString());
		}
	}
}