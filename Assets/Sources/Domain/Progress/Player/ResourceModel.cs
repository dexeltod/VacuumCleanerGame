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

		[SerializeField] private IntEntity _softCurrency;
		[SerializeField] private IntEntity _hardCurrency;

		[SerializeField] private IntEntity _cashScore;

		[SerializeField] private IntEntity _totalAmount;

		[SerializeField] private int _maxTotalResourceModifier;

		public ResourceModel(
			IntEntity softCurrency,
			IntEntity hardCurrency,
			IntEntity cashScore,
			IntEntity globalScore
		)
		{
			_hardCurrency = hardCurrency ?? throw new ArgumentNullException(nameof(hardCurrency));
			_softCurrency = softCurrency ?? throw new ArgumentNullException(nameof(softCurrency));
			_cashScore = cashScore ?? throw new ArgumentNullException(nameof(cashScore));
			_totalAmount = globalScore ?? throw new ArgumentNullException(nameof(globalScore));
		}

		public IReadOnlyProgress<int> CashScore => _cashScore;
		public IReadOnlyProgress<int> SoftCurrency => _softCurrency;
		public IReadOnlyProgress<int> TotalAmount => _totalAmount;
		public IReadOnlyProgress<int> HardCurrency => _hardCurrency;

		public int MaxTotalResourceCount => _maxTotalResourceModifier + GameConfig.DefaultMaxTotalResource;

		public int CurrentCashScore
		{
			get => _cashScore.Value;
			private set
			{
				if (value < 0)
					throw new ArgumentOutOfRangeException(nameof(value));

				_cashScore.Value = value;

				if (_cashScore.Value >= 0) return;

				_cashScore.Value = 0;

				throw new ArgumentOutOfRangeException(
					$"Current cash value {_cashScore.Value} is less than zero. It will be zero",
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
			if (CurrentCashScore + newCashScore >= _cashScore.ReadOnlyMaxValue)
				return false;

#if UNITY_EDITOR && DEV
			newCashScore *= MultiplyFactor;
#endif
			if (newCashScore <= 0) throw new ArgumentOutOfRangeException(nameof(newCashScore));

			CurrentCashScore += newCashScore;
			CurrentTotalResources += OnePoint;

			_cashScore.Value = CurrentCashScore;
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

			_softCurrency.Value -= newValue;

			return true;
		}

		public void ClearAllScores()
		{
			_cashScore.Value = 0;
			_totalAmount.Value = 0;
		}

		public void ClearTotalResources() => _totalAmount.Value = 0;
	}
}
