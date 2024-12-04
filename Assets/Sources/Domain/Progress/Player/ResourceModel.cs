using System;
using Sources.Domain.Progress.ResourcesData;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Utils.AssetPaths;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.Domain.Progress.Player
{
	[Serializable] public class ResourceModel : IResourceModelReadOnly, IResourceModel
	{
		private const int HundredPercent = 100;
		private const int OnePoint = 1;

		[SerializeField] private IntCurrency _softCurrency;
		[SerializeField] private IntCurrency _hardCurrency;

		[SerializeField] private IntCurrency _cashCashScore;

		[SerializeField] private IntCurrency _totalAmount;

		[FormerlySerializedAs("_maxGlobalScoreModifier")] [SerializeField]
		private int _maxTotalResourceModifier;

		private const int MultiplyFactor = 1;

		public ResourceModel(
			Resource<int> softCurrency,
			Resource<int> hardCurrency,
			Resource<int> cashScore,
			Resource<int> globalScore,
			int startScoreCount,
			int startCurrencyCount,
			int startGlobalScoreCount
		)
		{
			_hardCurrency = hardCurrency as IntCurrency ?? throw new ArgumentNullException(nameof(hardCurrency));
			_softCurrency = softCurrency as IntCurrency ?? throw new ArgumentNullException(nameof(softCurrency));
			_cashCashScore = cashScore as IntCurrency ?? throw new ArgumentNullException(nameof(cashScore));
			_totalAmount = globalScore as IntCurrency ?? throw new ArgumentNullException(nameof(globalScore));

			_cashCashScore.Set(startScoreCount);
			_hardCurrency.Set(startCurrencyCount);
			_softCurrency.Set(startCurrencyCount);
			_totalAmount.Set(startGlobalScoreCount);
		}

		public IReadOnlyProgressValue<int> CashScore => _cashCashScore;
		public IReadOnlyProgressValue<int> SoftCurrency => _softCurrency;
		public IReadOnlyProgressValue<int> TotalAmount => _totalAmount;
		public IReadOnlyProgressValue<int> HardCurrency => _hardCurrency;

		public int MaxTotalResourceCount => _maxTotalResourceModifier + GameConfig.DefaultMaxTotalResource;

		public int CurrentCashScore
		{
			get => _cashCashScore.Value;
			private set
			{
				if (value < 0)
					throw new ArgumentOutOfRangeException(nameof(value));

				_cashCashScore.Set(value);

				if (_cashCashScore.Value >= 0) return;

				_cashCashScore.Set(0);

				throw new ArgumentOutOfRangeException(
					$"Current cash value {_cashCashScore.Value} is less than zero. It will be zero",
					nameof(value)
				);
			}
		}

		public int CurrentTotalResources
		{
			get => _totalAmount.Value;
			private set => _totalAmount.Set(value);
		}

		public void AddScore(int newCashScore)
		{
#if UNITY_EDITOR
			newCashScore *= MultiplyFactor;
#endif
			if (newCashScore <= 0) throw new ArgumentOutOfRangeException(nameof(newCashScore));

			CurrentCashScore += newCashScore;
			CurrentTotalResources += OnePoint;

			_cashCashScore.Set(CurrentCashScore);
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
			_softCurrency.Set(newValue + _softCurrency.Value);
		}

		/// <summary>
		/// Decrease soft currency. Validates that there is enough money.	
		/// </summary>
		public bool TryDecreaseMoney(int newValue)
		{
			if (newValue < 0)
				throw new ArgumentOutOfRangeException(nameof(newValue));

			if (_softCurrency.Value - newValue < 0)
				throw new ArgumentOutOfRangeException(nameof(newValue), "Not enough money");

			_softCurrency.Set(_softCurrency.Value - newValue);

			return true;
		}

		public void ClearAllScores()
		{
			_cashCashScore.Set(0);
			_totalAmount.Set(0);
		}

		public void ClearTotalResources() =>
			_totalAmount.Set(0);
	}
}