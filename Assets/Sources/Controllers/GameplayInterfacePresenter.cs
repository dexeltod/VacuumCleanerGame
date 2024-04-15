using System;
using System.Collections;
using Graphic.Joystick_Pack.Scripts.Base;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using Sources.PresentationInterfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Controllers
{
	public class GameplayInterfacePresenter : Presenter, IGameplayInterfacePresenter
	{
		private readonly ICoroutineRunnerProvider _coroutineRunnerProvider;
		private readonly float _time;
		private readonly IGameplayInterfaceView _gameplayInterfaceView;
		private readonly ILevelChangerService _levelChangerService;
		private readonly ISpeedDecorator _speedDecorator;
		private readonly IPlayerStatSubscribable _maxCashScoreChangeable;
		private readonly int _cashScore;
		private readonly int _maxCashScore;

		public GameplayInterfacePresenter(
			ILevelChangerService levelChangerService,
			IGameplayInterfaceView gameplayInterfaceView,
			ISpeedDecorator speedDecorator,
			IPlayerStatSubscribable maxCashScoreChangeable,
			ICoroutineRunnerProvider coroutineRunnerProvider,
			float time,
			int cashScore,
			int maxCashScore
		)
		{
			_levelChangerService = levelChangerService ?? throw new ArgumentNullException(nameof(levelChangerService));
			_gameplayInterfaceView
				= gameplayInterfaceView ?? throw new ArgumentNullException(nameof(gameplayInterfaceView));
			_speedDecorator = speedDecorator ?? throw new ArgumentNullException(nameof(speedDecorator));
			_maxCashScoreChangeable = maxCashScoreChangeable ??
				throw new ArgumentNullException(nameof(maxCashScoreChangeable));
			_coroutineRunnerProvider = coroutineRunnerProvider ??
				throw new ArgumentNullException(nameof(coroutineRunnerProvider));
			if (time <= 0) throw new ArgumentOutOfRangeException(nameof(time));
			if (cashScore < 0) throw new ArgumentOutOfRangeException(nameof(cashScore));
			if (maxCashScore < 0) throw new ArgumentOutOfRangeException(nameof(maxCashScore));
			_time = time;
			_cashScore = cashScore;
			_maxCashScore = maxCashScore;
		}

		public Joystick Joystick => _gameplayInterfaceView.Joystick;

		private Button IncreaseSpeedButton => _gameplayInterfaceView.IncreaseSpeedButton;
		private Button GoToNextLevelButton => _gameplayInterfaceView.GoToNextLevelButton;

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

		public void SetTotalResourceCount(int globalScore) =>
			_gameplayInterfaceView.SetGlobalScore(globalScore);

		public void SetCashScore(int cashScore) =>
			_gameplayInterfaceView.SetCashScore(cashScore);

		public override void Enable()
		{
			_maxCashScoreChangeable.ValueChanged += OnMaxCashScoreChanged;
			IncreaseSpeedButton.onClick.AddListener(OnIncreaseSpeed);
			GoToNextLevelButton.onClick.AddListener(OnGoToNextLevel);

			_gameplayInterfaceView.SetCashScore(_cashScore);
		}

		public override void Disable()
		{
			_maxCashScoreChangeable.ValueChanged -= OnMaxCashScoreChanged;
			IncreaseSpeedButton.onClick.RemoveListener(OnIncreaseSpeed);
			GoToNextLevelButton.onClick.RemoveListener(OnGoToNextLevel);
		}

		private void OnMaxCashScoreChanged() =>
			_gameplayInterfaceView.SetMaxCashScore(_maxCashScore);

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