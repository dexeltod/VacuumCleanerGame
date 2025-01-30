using System;
using System.Collections;
using Sources.BusinessLogic.Interfaces;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.DomainInterfaces.Entities;
using Sources.PresentationInterfaces;
using Sources.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Controllers
{
	public class GameplayInterfacePresenter : Presenter, IGameplayInterfacePresenter
	{
		private readonly IReadOnlyProgress<int> _cashScore;
		private readonly ICoroutineRunner _coroutineRunnerProvider;
		private readonly IGameplayInterfaceView _gameplayInterfaceView;
		private readonly ILevelChangerService _levelChangerService;
		private readonly IStatReadOnly _maxCashScore;
		private readonly IReadOnlyProgress<int> _softCurrency;
		private readonly float _speedCooldown;
		private readonly ISpeedDecorator _speedDecorator;
		private readonly IReadOnlyProgress<int> _totalScore;

		public GameplayInterfacePresenter(
			IGameplayInterfaceView gameplayInterfaceView,
			ISpeedDecorator speedDecorator,
			ILevelChangerService levelChangerService,
			float speedCooldown,
			IReadOnlyProgress<int> cashScore,
			ICoroutineRunner coroutineRunnerProvider,
			IStatReadOnly maxCashScore,
			IReadOnlyProgress<int> totalScore,
			IReadOnlyProgress<int> softCurrency)
		{
			_maxCashScore = maxCashScore ?? throw new ArgumentNullException(nameof(maxCashScore));
			_totalScore = totalScore ?? throw new ArgumentNullException(nameof(totalScore));
			_softCurrency = softCurrency ?? throw new ArgumentNullException(nameof(softCurrency));
			_levelChangerService = levelChangerService ?? throw new ArgumentNullException(nameof(levelChangerService));
			_gameplayInterfaceView = gameplayInterfaceView ?? throw new ArgumentNullException(nameof(gameplayInterfaceView));
			_speedDecorator = speedDecorator ?? throw new ArgumentNullException(nameof(speedDecorator));
			_coroutineRunnerProvider = coroutineRunnerProvider ?? throw new ArgumentNullException(nameof(coroutineRunnerProvider));
			if (speedCooldown <= 0) throw new ArgumentOutOfRangeException(nameof(speedCooldown));

			_speedCooldown = speedCooldown;
			_cashScore = cashScore;
		}

		private Button IncreaseSpeedButton => _gameplayInterfaceView.IncreaseSpeedButton;

		private Button GoToNextLevelButton => _gameplayInterfaceView.GoToNextLevelButton;

		public void OnGoToNextLevel() => _levelChangerService.GoNextLevelWithReward();

		public override void Enable()
		{
			if (_totalScore.IsTotalScoreReached) _gameplayInterfaceView.GoToNextLevelButton.gameObject.SetActive(true);

			_maxCashScore.Changed += OnMaxCashScoreChanged;
			_softCurrency.Changed += OnSoftCuSetCurrencyChanged;
			_cashScore.Changed += OnCashScoreChanged;
			_totalScore.Changed += OnTotalScoreChanged;
			_totalScore.HalfReached += OnHalfScoreReached;

			_speedDecorator.Enable();

			IncreaseSpeedButton.onClick.AddListener(OnIncreaseSpeed);
			GoToNextLevelButton.onClick.AddListener(OnGoToNextLevel);

			_gameplayInterfaceView.SetCashScore(_cashScore.ReadOnlyValue);
			_gameplayInterfaceView.SetSoftCurrencyText(_softCurrency.ReadOnlyValue);
		}

		public override void Disable()
		{
			_cashScore.Changed -= OnCashScoreChanged;
			_maxCashScore.Changed -= OnMaxCashScoreChanged;
			_softCurrency.Changed -= OnSoftCuSetCurrencyChanged;
			_totalScore.HalfReached -= OnHalfScoreReached;

			_speedDecorator.Disable();

			IncreaseSpeedButton.onClick.RemoveListener(OnIncreaseSpeed);
			GoToNextLevelButton.onClick.RemoveListener(OnGoToNextLevel);
		}

		public void OnIncreaseSpeed()
		{
			if (_speedDecorator.IsDecorated)
				return;

			_coroutineRunnerProvider.Run(StartViewCooldownSpeedRoutine(_speedCooldown));
			_speedDecorator.Increase();
		}

		public void SetTotalResourceCount(int globalScore) => _gameplayInterfaceView.SetTotalResourceScore(globalScore);

		private void OnCashScoreChanged()
		{
			_gameplayInterfaceView.SetCashScore(_cashScore.ReadOnlyValue);
		}

		private void OnHalfScoreReached() => _gameplayInterfaceView.SetActiveGoToNextLevelButton(true);

		private void OnMaxCashScoreChanged() => _gameplayInterfaceView.SetMaxCashScore((int)_maxCashScore.Value);

		private void OnSoftCuSetCurrencyChanged()
		{
			_gameplayInterfaceView.SetSoftCurrencyText(_softCurrency.ReadOnlyValue);
		}

		private void OnTotalScoreChanged()
		{
			_gameplayInterfaceView.SetTotalResourceScore(_totalScore.ReadOnlyValue);
		}

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
