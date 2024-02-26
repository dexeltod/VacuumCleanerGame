using System;
using Joystick_Pack.Scripts.Base;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.PresentationInterfaces;

namespace Sources.Controllers
{
	public class GameplayInterfacePresenter : Presenter, IGameplayInterfacePresenter
	{
		private readonly ILevelChangerService _levelChangerService;
		private readonly IGameplayInterfaceView _gameplayInterfaceView;
		private readonly ISpeedDecorator _speedDecorator;
		private readonly int _cashScore;

		public GameplayInterfacePresenter(
			ILevelChangerService levelChangerService,
			IGameplayInterfaceView gameplayInterfaceView,
			ISpeedDecorator speedDecorator,
			int cashScore
		)
		{
			if (cashScore < 0) throw new ArgumentOutOfRangeException(nameof(cashScore));
			_levelChangerService = levelChangerService ?? throw new ArgumentNullException(nameof(levelChangerService));
			_gameplayInterfaceView
				= gameplayInterfaceView ?? throw new ArgumentNullException(nameof(gameplayInterfaceView));
			_speedDecorator = speedDecorator ?? throw new ArgumentNullException(nameof(speedDecorator));
			_cashScore = cashScore;
		}

		public Joystick Joystick => _gameplayInterfaceView.Joystick;

		public void GoToNextLevel() =>
			_levelChangerService.GoNextLevelWithReward();

		public void IncreaseSpeed() =>
			_speedDecorator.Increase();

		public void SetActiveGoToNextLevelButton(bool isActive) =>
			_gameplayInterfaceView.SetActiveGoToNextLevelButton(isActive);

		public void SetSoftCurrency(int soft) =>
			_gameplayInterfaceView.SetSoftCurrencyText(soft);

		public void SetGlobalScore(int globalScore) =>
			_gameplayInterfaceView.SetGlobalScore(globalScore);

		public void SetCashScore(int cashScore) =>
			_gameplayInterfaceView.SetCashScore(cashScore);

		public override void Enable()
		{
			_gameplayInterfaceView.Enable();
			_gameplayInterfaceView.SetCashScore(_cashScore);
		}

		public override void Disable() =>
			_gameplayInterfaceView.Disable();
	}
}