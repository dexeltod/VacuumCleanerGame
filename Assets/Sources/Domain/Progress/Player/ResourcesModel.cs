using System;
using Sources.Domain.Progress.ResourcesData;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Utils.Configs.Scripts;
using UnityEngine;

namespace Sources.Domain.Progress.Player
{
	[Serializable] public class ResourcesModel : IResourcesModel
	{
		private const int HundredPercent = 100;

		[SerializeField] private IntResource _softCurrency;
		[SerializeField] private IntResource _hardCurrency;
		[SerializeField] private IntResource _cashScore;
		[SerializeField] private IntResource _globalScore;

		[SerializeField] private int _maxCashScoreModifier;
		[SerializeField] private int _maxGlobalScoreModifier;

		public ResourcesModel(
			Resource<int> softCurrency,
			Resource<int> hardCurrency,
			Resource<int> cashScore,
			Resource<int> globalScore,
			int startCount,
			int globalScoreCount
		)
		{
			_hardCurrency = hardCurrency as IntResource ?? throw new ArgumentNullException(nameof(hardCurrency));
			_softCurrency = softCurrency as IntResource ?? throw new ArgumentNullException(nameof(softCurrency));
			_cashScore = cashScore as IntResource ?? throw new ArgumentNullException(nameof(cashScore));
			_globalScore = globalScore as IntResource ?? throw new ArgumentNullException(nameof(globalScore));

			_cashScore.Set(startCount);
			_hardCurrency.Set(startCount);
			_softCurrency.Set(startCount);
			_globalScore.Set(globalScoreCount);
		}

		public IResourceReadOnly<int> Score => _cashScore;
		public IResourceReadOnly<int> SoftCurrency => _softCurrency;
		public IResourceReadOnly<int> GlobalScore => _globalScore;
		public IResourceReadOnly<int> HardCurrency => _hardCurrency;

		public int PercentOfScore => GetPercentMaxCashScore();

		public int MaxCashScore => _maxCashScoreModifier + GameConfig.DefaultMaxSandFillCount;
		public int MaxGlobalScore => _maxGlobalScoreModifier + GameConfig.DefaultMaxGlobalScore;

		public int CurrentCashScore
		{
			get => _cashScore.Count;
			private set => _cashScore.Set(value);
		}

		public int GlobalSandCount
		{
			get => _globalScore.Count;
			private set => _globalScore.Set(value);
		}

		public void AddCashScore(int newValue)
		{
			if (newValue <= 0) throw new ArgumentOutOfRangeException(nameof(newValue));
			CurrentCashScore += newValue;
			GlobalSandCount += newValue;

			_cashScore.Set(CurrentCashScore);
		}

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

		public void ClearScores()
		{
			_cashScore.Set(0);
			_globalScore.Set(0);
		}

		private int GetPercentMaxCashScore() =>
			(_cashScore.Count / HundredPercent) * MaxCashScore;
	}
}