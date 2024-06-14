using Sources.ControllersInterfaces;
using Sources.PresentationInterfaces.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.PresentationInterfaces
{
	public interface IMainMenuView : IPresentableView<IMainMenuPresenter>
	{
		Button PlayButton { get; }
		Button DeleteSavesButton { get; }
		Button AddScoreButton { get; }
		Button LeaderboardButton { get; }
		Button SettingsButton { get; }
		GameObject LeaderBoardView { get; }
		Button AddButton { get; }
	}
}