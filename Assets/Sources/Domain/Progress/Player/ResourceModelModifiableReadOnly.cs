using System;
using Sources.Domain.Progress.ResourcesData;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Utils;
using UnityEngine;

namespace Sources.Domain.Progress.Player
{
	[Serializable] public class ResourceModelModifiableReadOnly : IResourceModelReadOnly, IResourceModelModifiable
	{
		private const int HundredPercent = 100;
		private const int OnePoint = 1;

		[SerializeField] private IntResource _softCurrency;
		[SerializeField] private IntResource _hardCurrency;

		[SerializeField] private IntResource _cashScore;
		[SerializeField] private IntResource _totalResourcesAmount;

		[SerializeField] private int _maxCashScoreModifier;
		[SerializeField] private int _maxGlobalScoreModifier;

		private const int MultiplyFactor = 1;

		public ResourceModelModifiableReadOnly(
			Resource<int> softCurrency,
			Resource<int> hardCurrency,
			Resource<int> cashScore,
			Resource<int> globalScore,
			int startScoreCount,
			int startCurrencyCount,
			int startGlobalScoreCount
		)
		{
			_hardCurrency = hardCurrency as IntResource ?? throw new ArgumentNullException(nameof(hardCurrency));
			_softCurrency = softCurrency as IntResource ?? throw new ArgumentNullException(nameof(softCurrency));
			_cashScore = cashScore as IntResource ?? throw new ArgumentNullException(nameof(cashScore));
			_totalResourcesAmount = globalScore as IntResource ?? throw new ArgumentNullException(nameof(globalScore));

			_cashScore.Set(startScoreCount);
			_hardCurrency.Set(startCurrencyCount);
			_softCurrency.Set(startCurrencyCount);
			_totalResourcesAmount.Set(startGlobalScoreCount);
		}

		public IResourceReadOnly<int> Score => _cashScore;
		public IResourceReadOnly<int> SoftCurrency => _softCurrency;
		public IResourceReadOnly<int> TotalResourcesAmount => _totalResourcesAmount;
		public IResourceReadOnly<int> HardCurrency => _hardCurrency;

		public int PercentOfScore => (_cashScore.Count / HundredPercent) * MaxCashScore;
		public int MaxCashScore => _maxCashScoreModifier + GameConfig.DefaultMaxSandFillCount;
		public int MaxTotalResourceCount => _maxGlobalScoreModifier + GameConfig.DefaultMaxTotalResource;

		public int CurrentCashScore
		{
			get => _cashScore.Count;
			private set => _cashScore.Set(value);
		}

		public int CurrentTotalResources
		{
			get => _totalResourcesAmount.Count;
			private set => _totalResourcesAmount.Set(value);
		}

		public void AddMaxCashScoreModifier(int newAmount) =>
			_maxCashScoreModifier += newAmount;

		public void AddScore(int newCashScore)
		{
#if UNITY_EDITOR
			newCashScore *= MultiplyFactor;
#endif
			if (newCashScore <= 0) throw new ArgumentOutOfRangeException(nameof(newCashScore));

			CurrentCashScore += newCashScore;
			CurrentTotalResources += OnePoint;

			_cashScore.Set(CurrentCashScore);
		}

		public void AddMaxTotalResourceModifier(int newAmount) =>
			_maxGlobalScoreModifier += newAmount;

		public void DecreaseCashScore(int newValue)
		{
			if (newValue <= 0) throw new ArgumentOutOfRangeException(nameof(newValue));
			CurrentCashScore -= newValue;
		}

		public void AddMoney(int newValue)
		{
			if (newValue <= 0) throw new ArgumentOutOfRangeException(nameof(newValue));
			_softCurrency.Set(newValue + _softCurrency.Count);
		}

		public void DecreaseMoney(int newValue)
		{
			if (newValue < 0) throw new ArgumentOutOfRangeException(nameof(newValue));
			_softCurrency.Set(_softCurrency.Count - newValue);
		}

		public void ClearAllScores()
		{
			_cashScore.Set(0);
			_totalResourcesAmount.Set(0);
		}

		public void ClearTotalResources() =>
			_totalResourcesAmount.Set(0);
	}
}