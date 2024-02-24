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
		private readonly ISpeedDecorator _speedDecorator;

		public GameplayInterfacePresenter(
			ILevelChangerService levelChangerService,
			IGameplayInterfaceView gameplayInterfaceView,
			ISpeedDecorator speedDecorator
		)
		{
			_levelChangerService = levelChangerService ?? throw new ArgumentNullException(nameof(levelChangerService));
			_gameplayInterfaceView
				= gameplayInterfaceView ?? throw new ArgumentNullException(nameof(gameplayInterfaceView));
			_speedDecorator = speedDecorator ?? throw new ArgumentNullException(nameof(speedDecorator));
		}

		public void GoToNextLevel() =>
			_levelChangerService.GoNextLevelWithReward();

		public void IncreaseSpeed() =>
			_speedDecorator.Increase();

		public override void Enable() =>
			_gameplayInterfaceView.Enable();

		public override void Disable() =>
			_gameplayInterfaceView.Disable();
	}
}