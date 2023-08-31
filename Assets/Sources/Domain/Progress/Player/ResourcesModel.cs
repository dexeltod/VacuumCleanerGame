using System;
using Newtonsoft.Json;
using Sources.Domain.Progress.ResourcesData;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Utils.Configs;
using UnityEngine;

namespace Sources.Domain.Progress.Player
{
	[Serializable]
	public class ResourcesModel : IResourcesModel
	{
		[SerializeField] private IntResource _softCurrency;
		[SerializeField] private IntResource _hardCurrency;
		[SerializeField] private int _maxFillModifier;
		[SerializeField] private int _currentSandCount;

		[JsonProperty(nameof(SoftCurrency))]
		public IResource<int> SoftCurrency => _softCurrency;

		[JsonProperty(nameof(HardCurrency))]
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

		[JsonConstructor]
		public ResourcesModel(Resource<int> softCurrency, Resource<int> hardCurrency, int startCount)
		{
			_hardCurrency = hardCurrency as IntResource;
			HardCurrency.Set(startCount);

			_softCurrency = softCurrency as IntResource;
			SoftCurrency.Set(startCount);
		}

		public void AddSand(int count) =>
			CurrentSandCount += count;

		public void DecreaseSand(int count) =>
			CurrentSandCount -= count;

		public void AddMoney(int count) =>
			SoftCurrency.Set(count + _softCurrency.Count);

		public void DecreaseMoney(int count) =>
			SoftCurrency.Set(count - _softCurrency.Count);
	}
}