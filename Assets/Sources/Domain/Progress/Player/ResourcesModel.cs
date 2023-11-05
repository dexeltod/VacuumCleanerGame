using System;
using Newtonsoft.Json;
using Sources.Domain.Progress.ResourcesData;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Utils.Configs;
using UnityEngine;

namespace Sources.Domain.Progress.Player
{
	[Serializable] public class ResourcesModel : IResourcesModel
	{
		[SerializeField] private IntResource _softCurrency;
		[SerializeField] private IntResource _hardCurrency;
		[SerializeField] private IntResource _cashScore;
		[SerializeField] private IntResource _globalScore;

		[SerializeField] private int _maxModifier;

		public IResourceReadOnly<int> Score        => _cashScore;
		public IResourceReadOnly<int> SoftCurrency => _softCurrency;
		public IResourceReadOnly<int> GlobalScore  => _globalScore;
		public IResourceReadOnly<int> HardCurrency => _hardCurrency;

		public int PercentOfScore => GetPercentOfScore();

		[JsonProperty(nameof(MaxCashScore))]
		public int MaxCashScore => MaxModifier + GameConfig.DefaultMaxSandFillCount;

		public int MaxModifier
		{
			get => _maxModifier;
			private set => _maxModifier = value;
		}

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

		public ResourcesModel
		(
			Resource<int> softCurrency,
			Resource<int> hardCurrency,
			Resource<int> cashScore,
			Resource<int> globalScore,
			int           startCount,
			int           globalScoreCount
		)
		{
			_hardCurrency = hardCurrency as IntResource;
			_softCurrency = softCurrency as IntResource;
			_cashScore    = cashScore as IntResource;
			_globalScore  = globalScore as IntResource;

			_hardCurrency.Set(startCount);
			_softCurrency.Set(startCount);
			_globalScore.Set(globalScoreCount);
		}

		public void SetCashScore(int newValue) =>
			_cashScore.Set(newValue);

		public void AddCashScore(int newValue)
		{
			CurrentCashScore += newValue;
			GlobalSandCount  += newValue;

			_cashScore.Set(CurrentCashScore);
		}

		public void DecreaseCashScore(int newValue) =>
			CurrentCashScore -= newValue;

		public void AddMoney(int newValue) =>
			_softCurrency.Set(newValue + _softCurrency.Count);

		public void DecreaseMoney(int newValue) =>
			_softCurrency.Set(_softCurrency.Count - newValue);

		public void ClearScores()
		{
			_cashScore.Set(0);
			_globalScore.Set(0);
		}

		private int GetPercentOfScore()
		{
			return (_cashScore.Count / 100) * MaxCashScore;
		}
	}
}