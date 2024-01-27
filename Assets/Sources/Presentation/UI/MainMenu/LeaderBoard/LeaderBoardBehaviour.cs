using UnityEngine;

namespace Sources.Presentation.UI.MainMenu.LeaderBoard
{
	public class LeaderBoardBehaviour : MonoBehaviour
	{
		[SerializeField] private LeaderBoardPlayerPanelBehaviour _playerPanel;
		[SerializeField] private GameObject _leaderBoardContainer;

		public Transform Container => _leaderBoardContainer.transform;
		public LeaderBoardPlayerPanelBehaviour PlayerPanel => _playerPanel;
	}
}