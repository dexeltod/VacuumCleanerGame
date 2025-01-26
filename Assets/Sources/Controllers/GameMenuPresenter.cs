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
		private readonly IStateMachine _stateMachine;

		public GameMenuPresenter(
			IGameMenuView gameMenu,
			IStateMachine stateMachine
		)
		{
			_gameMenu = gameMenu ?? throw new ArgumentNullException(nameof(gameMenu));
			_stateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
		}

		public void Dispose() => Disable();

		public void GoToMainMenu() => _stateMachine.Enter<IMenuState>();

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
	}
}