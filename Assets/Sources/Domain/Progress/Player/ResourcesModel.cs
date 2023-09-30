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

		[SerializeField] private int _maxFillModifier;
		[SerializeField] private int _currentSandCount;
		[SerializeField] private int _globalSandCount;

		public IResource<int> Score => _score;

		public IResource<int> SoftCurrency => _softCurrency;
		public IResource<int> HardCurrency => _hardCurrency;

		[JsonProperty(nameof(MaxFilledScore))]
		public int MaxFilledScore => MaxFillModifier + GameConfig.DefaultMaxSandFillCount;

		public int MaxFillModifier
		{
			get => _maxFillModifier;
			private set => _maxFillModifier = value;
		}

		public int CurrentSandCount
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

		public void AddSand(int count)
		{
			CurrentSandCount += count;
			GlobalSandCount  += count;
		}

		public void DecreaseSand(int count) =>
			CurrentSandCount -= count;

		public void AddMoney(int count) =>
			SoftCurrency.Set(count + _softCurrency.Count);

		public void DecreaseMoney(int count) =>
			SoftCurrency.Set(count - _softCurrency.Count);
	}
}