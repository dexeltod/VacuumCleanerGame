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
		[SerializeField] private IntResource _score;

		[SerializeField] private int _maxModifier;
		[SerializeField] private int _currentSandCount;
		[SerializeField] private int _globalSandCount;

		public IResource<int> Score => _score;

		public IResource<int> SoftCurrency => _softCurrency;
		public IResource<int> HardCurrency => _hardCurrency;

		[JsonProperty(nameof(MaxCashScore))]
		public int MaxCashScore => MaxModifier + GameConfig.DefaultMaxSandFillCount;

		public int MaxModifier
		{
			get => _maxModifier;
			private set => _maxModifier = value;
		}

		public int CurrentCashScore
		{
			get => _currentSandCount;
			private set => _currentSandCount = value;
		}

		public int GlobalSandCount
		{
			get => _globalSandCount;
			private set => _globalSandCount = value;
		}

		public ResourcesModel
		(
			Resource<int> softCurrency,
			Resource<int> hardCurrency,
			Resource<int> score,
			int           startCount,
			int           globalSandCount
		)
		{
			_hardCurrency = hardCurrency as IntResource;
			_softCurrency = softCurrency as IntResource;
			_score        = score as IntResource;

			HardCurrency.Set(startCount);
			SoftCurrency.Set(startCount);

			_globalSandCount = globalSandCount;
		}

		public void AddSand(int newValue)
		{
			CurrentCashScore += newValue;
			GlobalSandCount  += newValue;
			_score.Set(_score.Count + newValue);
		}

		public void DecreaseSand(int newValue) =>
			CurrentCashScore -= newValue;

		public void AddMoney(int newValue) =>
			SoftCurrency.Set(newValue + _softCurrency.Count);

		public void DecreaseMoney(int newValue) =>
			SoftCurrency.Set(newValue - _softCurrency.Count);
	}
}