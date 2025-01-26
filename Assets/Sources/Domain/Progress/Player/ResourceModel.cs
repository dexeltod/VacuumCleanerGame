using System;
using Sources.Domain.Progress.Entities.Values;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Utils.AssetPaths;
using UnityEngine;

namespace Sources.Domain.Progress.Player
{
	[Serializable]
	public class ResourceModel : IResourceModel
	{
		private const int OnePoint = 1;

		private const int MultiplyFactor = 1;

		[SerializeField] private IntEntityValue _softCurrency;
		[SerializeField] private IntEntityValue _hardCurrency;

		[SerializeField] private IntEntityValue _cashCashScore;

		[SerializeField] private IntEntityValue _totalAmount;

		[SerializeField] private int _maxTotalResourceModifier;

		public ResourceModel(
			IntEntityValue softCurrency,
			IntEntityValue hardCurrency,
			IntEntityValue cashScore,
			IntEntityValue globalScore,
			int startScoreCount,
			int startCurrencyCount,
			int startGlobalScoreCount
		)
		{
			_hardCurrency = hardCurrency ?? throw new ArgumentNullException(nameof(hardCurrency));
			_softCurrency = softCurrency ?? throw new ArgumentNullException(nameof(softCurrency));
			_cashCashScore = cashScore ?? throw new ArgumentNullException(nameof(cashScore));
			_totalAmount = globalScore ?? throw new ArgumentNullException(nameof(globalScore));

			_cashCashScore.Value = startCurrencyCount;
			_hardCurrency.Value = startCurrencyCount;
			_softCurrency.Value = startCurrencyCount;
			_totalAmount.Value = startGlobalScoreCount;
		}

		public IReadOnlyProgress<int> CashScore => _cashCashScore;
		public IReadOnlyProgress<int> SoftCurrency => _softCurrency;
		public IReadOnlyProgress<int> TotalAmount => _totalAmount;
		public IReadOnlyProgress<int> HardCurrency => _hardCurrency;
		public event Action HalfScoreReached;

		public int MaxTotalResourceCount => _maxTotalResourceModifier + GameConfig.DefaultMaxTotalResource;

		public int CurrentCashScore
		{
			get => _cashCashScore.Value;
			private set
			{
				if (value < 0)
					throw new ArgumentOutOfRangeException(nameof(value));

				_cashCashScore.Value = value;

				if (_cashCashScore.Value >= 0) return;

				_cashCashScore.Value = 0;

				throw new ArgumentOutOfRangeException(
					$"Current cash value {_cashCashScore.Value} is less than zero. It will be zero",
					nameof(value)
				);
			}
		}

		public int CurrentTotalResources
		{
			get => _totalAmount.Value;
			private set => _totalAmount.Value = value;
		}

		public bool TryAddScore(int newCashScore)
		{
			if (CurrentCashScore + newCashScore >= _cashCashScore.ReadOnlyMaxValue)
				return false;

#if UNITY_EDITOR && DEV
			newCashScore *= MultiplyFactor;
#endif
			if (newCashScore <= 0) throw new ArgumentOutOfRangeException(nameof(newCashScore));

			CurrentCashScore += newCashScore;
			CurrentTotalResources += OnePoint;

			_cashCashScore.Value = CurrentCashScore;
			return true;
		}

		public void AddMaxTotalResourceModifier(int newAmount)
		{
			if (newAmount <= 0) throw new ArgumentOutOfRangeException(nameof(newAmount));

			_maxTotalResourceModifier += newAmount;
		}

		public void DecreaseCashScore(int newValue)
		{
			if (newValue <= 0) throw new ArgumentOutOfRangeException(nameof(newValue));

			CurrentCashScore -= newValue;
		}

		public void AddMoney(int newValue)
		{
			if (newValue <= 0) throw new ArgumentOutOfRangeException(nameof(newValue));

			_softCurrency.Value = newValue + _softCurrency.Value;
		}

		public void SetMoney(int value)
		{
			if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));

			_softCurrency.Value = value;
		}

		/// <summary>
		///     Decrease soft currency. Validates that there is enough money.
		/// </summary>
		public bool TryDecreaseMoney(int newValue)
		{
			if (newValue < 0)
				throw new ArgumentOutOfRangeException(nameof(newValue));

			if (_softCurrency.Value - newValue < 0)
				throw new ArgumentOutOfRangeException(nameof(newValue), "Not enough money");

			_softCurrency.Value = _softCurrency.Value - newValue;

			return true;
		}

		public void ClearAllScores()
		{
			_cashCashScore.Value = 0;
			_totalAmount.Value = 0;
		}

		public void ClearTotalResources() => _totalAmount.Value = 0;
	}
}