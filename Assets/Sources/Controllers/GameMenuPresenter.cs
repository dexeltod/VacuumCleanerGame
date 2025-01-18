using System;
using Sources.BusinessLogic.Services;
using Sources.BusinessLogic.States;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.PresentationInterfaces;

namespace Sources.Controllers
{
	public class GameMenuPresenter : Presenter, IGameMenuPresenter, IDisposable
	{
		private readonly IGameMenuView _gameMenu;
		private readonly IGameStateChanger _gameStateChanger;

		public GameMenuPresenter(
			IGameMenuView gameMenu,
			IGameStateChanger gameStateChanger
		)
		{
			_gameMenu = gameMenu ?? throw new ArgumentNullException(nameof(gameMenu));
			_gameStateChanger = gameStateChanger ?? throw new ArgumentNullException(nameof(gameStateChanger));
		}

		public void GoToMainMenu() =>
			_gameStateChanger.Enter<IMenuState>();

		public override void Enable()
		{
			_gameMenu.OpenMenuButton.onClick.AddListener(_gameMenu.Disable);
			_gameMenu.OpenMenuButton.onClick.AddListener(_gameMenu.Enable);
		}

		public override void Disable()
		{
			_gameMenu.OpenMenuButton.onClick.RemoveListener(_gameMenu.Disable);
			_gameMenu.OpenMenuButton.onClick.RemoveListener(_gameMenu.Enable);
		}

		public void Dispose() =>
			Disable();
	}
}
