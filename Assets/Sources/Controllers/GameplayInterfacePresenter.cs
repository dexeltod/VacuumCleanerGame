using System;
using System.Collections;
using Graphic.Joystick_Pack.Scripts.Base;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using Sources.PresentationInterfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Controllers
{
	public class GameplayInterfacePresenter : Presenter, IGameplayInterfacePresenter
	{
		private readonly int _cashScore;
		private readonly ICoroutineRunnerProvider _coroutineRunnerProvider;
		private readonly float _time;
		private readonly IGameplayInterfaceView _gameplayInterfaceView;
		private readonly ILevelChangerService _levelChangerService;
		private readonly ISpeedDecorator _speedDecorator;

		public GameplayInterfacePresenter(
			ILevelChangerService levelChangerService,
			IGameplayInterfaceView gameplayInterfaceView,
			ISpeedDecorator speedDecorator,
			int cashScore,
			ICoroutineRunnerProvider coroutineRunnerProvider,
			float time
		)
		{
			if (cashScore < 0) throw new ArgumentOutOfRangeException(nameof(cashScore));
			if (time <= 0) throw new ArgumentOutOfRangeException(nameof(time));
			_levelChangerService = levelChangerService ?? throw new ArgumentNullException(nameof(levelChangerService));
			_gameplayInterfaceView
				= gameplayInterfaceView ?? throw new ArgumentNullException(nameof(gameplayInterfaceView));
			_speedDecorator = speedDecorator ?? throw new ArgumentNullException(nameof(speedDecorator));
			_cashScore = cashScore;
			_coroutineRunnerProvider = coroutineRunnerProvider ??
				throw new ArgumentNullException(nameof(coroutineRunnerProvider));
			_time = time;
		}

		private Button IncreaseSpeedButton => _gameplayInterfaceView.IncreaseSpeedButton;
		private Button GoToNextLevelButton => _gameplayInterfaceView.GoToNextLevelButton;

		public Joystick Joystick => _gameplayInterfaceView.Joystick;

		public void OnGoToNextLevel() =>
			_levelChangerService.GoNextLevelWithReward();

		public void OnIncreaseSpeed()
		{
			if (_speedDecorator.IsDecorated != false) return;
			_coroutineRunnerProvider.Implementation.Run(StartCooldownSpeedRoutine(_time));
			_speedDecorator.Increase();
		}

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
			IncreaseSpeedButton.onClick.AddListener(OnIncreaseSpeed);
			GoToNextLevelButton.onClick.AddListener(OnGoToNextLevel);

			_gameplayInterfaceView.SetCashScore(_cashScore);
		}

		public override void Disable()
		{
			IncreaseSpeedButton.onClick.RemoveListener(OnIncreaseSpeed);
			GoToNextLevelButton.onClick.RemoveListener(OnGoToNextLevel);
		}

		public void StartCooldownSpeedButton(float time) =>
			_coroutineRunnerProvider.Implementation.Run(StartCooldownSpeedRoutine(time));

		private IEnumerator StartCooldownSpeedRoutine(float time)
		{
			_gameplayInterfaceView.FillSpeedButtonImage(0);

			float timer = 0;

			while (timer <= time)
			{
				timer += Time.deltaTime;
				_gameplayInterfaceView.FillSpeedButtonImage(timer / time);

				yield return null;
			}
		}
	}
}