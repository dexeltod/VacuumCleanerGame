using System;
using System.Collections;
using Plugins.Joystick_Pack.Scripts.Base;
using Sources.BusinessLogic.Interfaces;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.Entities;
using Sources.PresentationInterfaces;
using Sources.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Controllers
{
	public class GameplayInterfacePresenter : Presenter, IGameplayInterfacePresenter
	{
		private readonly int _cashScore;
		private readonly ICoroutineRunner _coroutineRunnerProvider;
		private readonly IGameplayInterfaceView _gameplayInterfaceView;
		private readonly ILevelChangerService _levelChangerService;
		private readonly IStatReadOnly _maxCashScore;
		private readonly ISpeedDecorator _speedDecorator;
		private readonly float _time;

		public GameplayInterfacePresenter(IGameplayInterfaceView gameplayInterfaceView,
			ISpeedDecorator speedDecorator,
			ILevelChangerService levelChangerService,
			float time,
			int cashScore,
			ICoroutineRunner coroutineRunnerProvider,
			IStatReadOnly maxCashScore)
		{
			_maxCashScore = maxCashScore ?? throw new ArgumentNullException(nameof(maxCashScore));
			_levelChangerService = levelChangerService ?? throw new ArgumentNullException(nameof(levelChangerService));
			_gameplayInterfaceView = gameplayInterfaceView ?? throw new ArgumentNullException(nameof(gameplayInterfaceView));
			_speedDecorator = speedDecorator ?? throw new ArgumentNullException(nameof(speedDecorator));
			_coroutineRunnerProvider = coroutineRunnerProvider ?? throw new ArgumentNullException(nameof(coroutineRunnerProvider));
			if (time <= 0) throw new ArgumentOutOfRangeException(nameof(time));
			if (cashScore < 0) throw new ArgumentOutOfRangeException(nameof(cashScore));

			_time = time;
			_cashScore = cashScore;
		}

		private Button IncreaseSpeedButton => _gameplayInterfaceView.IncreaseSpeedButton;
		private Button GoToNextLevelButton => _gameplayInterfaceView.GoToNextLevelButton;

		public Joystick Joystick => _gameplayInterfaceView.Joystick;

		public void OnGoToNextLevel() =>
			_levelChangerService.GoNextLevelWithReward();

		public void OnIncreaseSpeed()
		{
			if (_speedDecorator.IsDecorated)
				return;

			_coroutineRunnerProvider.Run(StartViewCooldownSpeedRoutine(_time));
			_speedDecorator.Increase();
		}

		public void SetActiveGoToNextLevelButton(bool isActive) =>
			_gameplayInterfaceView.SetActiveGoToNextLevelButton(
				isActive
			);

		public void SetSoftCurrency(int soft) =>
			_gameplayInterfaceView.SetSoftCurrencyText(
				soft
			);

		public void SetTotalResourceCount(int globalScore) =>
			_gameplayInterfaceView.SetTotalResourceScore(
				globalScore
			);

		public void SetCashScore(int cashScore) =>
			_gameplayInterfaceView.SetCashScore(cashScore);

		public override void Enable()
		{
			_maxCashScore.Changed += OnMaxCashScoreChanged;
			_speedDecorator.Enable();

			IncreaseSpeedButton.onClick.AddListener(OnIncreaseSpeed);
			GoToNextLevelButton.onClick.AddListener(OnGoToNextLevel);
			_gameplayInterfaceView.SetCashScore(_cashScore);
		}

		public override void Disable()
		{
			_maxCashScore.Changed -= OnMaxCashScoreChanged;
			_speedDecorator.Disable();

			IncreaseSpeedButton.onClick.RemoveListener(OnIncreaseSpeed);
			GoToNextLevelButton.onClick.RemoveListener(OnGoToNextLevel);
		}

		private void OnMaxCashScoreChanged() =>
			_gameplayInterfaceView.SetMaxCashScore(
				(int)_maxCashScore.Value
			);

		private IEnumerator StartViewCooldownSpeedRoutine(float time)
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
