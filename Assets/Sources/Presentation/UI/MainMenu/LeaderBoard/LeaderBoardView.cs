using Sources.ControllersInterfaces;
using Sources.Presentation.Common;
using UnityEngine;

namespace Sources.Presentation.UI.MainMenu.LeaderBoard
{
	public class LeaderBoardView : PresentableView<IMainMenuPresenter>, ILeaderBoardView
	{
		[SerializeField] private LeaderBoardPlayerPanelBehaviour _playerPanel;
		[SerializeField] private GameObject _leaderBoardContainer;

		public Transform Container => _leaderBoardContainer.transform;
		public LeaderBoardPlayerPanelBehaviour PlayerPanel => _playerPanel;

		public new void Construct(IMainMenuPresenter presenter) =>
			base.Construct(presenter);
	}
}