using System;
using Sources.Application.Utils.Configs;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.Domain.Progress.Player
{
	[Serializable]
	public class ResourcesData : IResourcesData
	{
		public IResource<int> SoftCurrency { get; private set; }

		public IResource<int> HardCurrency { get; private set; }
		public int MaxFilledScore => MaxFillModifier + GameConfig.DefaultMaxSandFillCount;
		public int MaxFillModifier { get; private set; } = 0;
		public int CurrentSandCount { get; private set; } = 0;

		public ResourcesData(IResource<int> softCurrency, IResource<int> hardCurrency, int startCount)
		{
			HardCurrency = hardCurrency;
			HardCurrency.Set(startCount);

			SoftCurrency = softCurrency;
			SoftCurrency.Set(startCount);
		}

		public void AddSand(int count) =>
			CurrentSandCount += count;

		public void DecreaseSand(int count) =>
			CurrentSandCount -= count;

		public void AddMoney(int count) =>
			SoftCurrency.Increase(count);

		public void DecreaseMoney(int count) =>
			SoftCurrency.Decrease(count);
	}
}