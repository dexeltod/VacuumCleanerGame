using System;
using System.Collections;
using Graphic.Joystick_Pack.Scripts.Base;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.DomainInterfaces.Entities;
using Sources.InfrastructureInterfaces.Providers;
using Sources.PresentationInterfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Controllers
{
	public class GameplayInterfacePresenter : Presenter, IGameplayInterfacePresenter
	{
		private readonly IStatReadOnly _maxCashScore;
		private readonly ICoroutineRunnerProvider _coroutineRunnerProvider;
		private readonly float _time;
		private readonly IGameplayInterfaceView _gameplayInterfaceView;
		private readonly ILevelChangerService _levelChangerService;
		private readonly ISpeedDecorator _speedDecorator;
		private readonly int _cashScore;

		public GameplayInterfacePresenter(
			ILevelChangerService levelChangerService,
			IGameplayInterfaceView gameplayInterfaceView,
			ISpeedDecorator speedDecorator,
			ICoroutineRunnerProvider coroutineRunnerProvider,
			float time,
			int cashScore,
			IStatReadOnly maxCashScore
		)
		{
			_maxCashScore = maxCashScore ??
				throw new ArgumentNullException(nameof(maxCashScore));
			_levelChangerService = levelChangerService ?? throw new ArgumentNullException(nameof(levelChangerService));
			_gameplayInterfaceView
				= gameplayInterfaceView ?? throw new ArgumentNullException(nameof(gameplayInterfaceView));
			_speedDecorator = speedDecorator ?? throw new ArgumentNullException(nameof(speedDecorator));
			_coroutineRunnerProvider = coroutineRunnerProvider ??
				throw new ArgumentNullException(nameof(coroutineRunnerProvider));
			if (time <= 0) throw new ArgumentOutOfRangeException(nameof(time));
			if (cashScore < 0) throw new ArgumentOutOfRangeException(nameof(cashScore));
			_time = time;
			_cashScore = cashScore;
		}

		public Joystick Joystick => _gameplayInterfaceView.Joystick;

		private Button IncreaseSpeedButton => _gameplayInterfaceView.IncreaseSpeedButton;
		private Button GoToNextLevelButton => _gameplayInterfaceView.GoToNextLevelButton;

		public void OnGoToNextLevel() =>
			_levelChangerService.GoNextLevelWithReward();

		public void OnIncreaseSpeed()
		{
			if (_speedDecorator.IsDecorated != false)
				return;

			_coroutineRunnerProvider.Self.Run(StartViewCooldownSpeedRoutine(_time));
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
			_gameplayInterfaceView.SetMaxCashScore((int)_maxCashScore.Value);

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