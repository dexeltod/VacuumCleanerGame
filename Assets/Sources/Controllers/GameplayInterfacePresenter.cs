using System;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.PresentationInterfaces;

namespace Sources.Controllers
{
	public class GameplayInterfacePresenter : Presenter, IGameplayInterfacePresenter
	{
		private readonly ILevelChangerService _levelChangerService;
		private readonly IGameplayInterfaceView _gameplayInterfaceView;

		public GameplayInterfacePresenter(
			ILevelChangerService levelChangerService,
			IGameplayInterfaceView gameplayInterfaceView
		)
		{
			_levelChangerService = levelChangerService ?? throw new ArgumentNullException(nameof(levelChangerService));
			_gameplayInterfaceView
				= gameplayInterfaceView ?? throw new ArgumentNullException(nameof(gameplayInterfaceView));
		}

		public void GoToNextLevel() =>
			_levelChangerService.GoNextLevelWithReward();

		public override void Enable() =>
			_gameplayInterfaceView.Enable();

		public override void Disable() =>
			_gameplayInterfaceView.Disable();
	}
}