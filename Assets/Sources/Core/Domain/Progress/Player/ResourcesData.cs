using System;
using Sources.Core.Domain.DomainInterfaces;
using Sources.Core.Domain.DomainInterfaces.DomainServicesInterfaces;
using Sources.Core.Utils.Configs;

namespace Sources.Core.Domain.Progress.Player
{
	[Serializable]
	public class ResourcesData : IResourcesData
	{
		public IResource<int> SoftCurrency { get; private set; }
		public int MaxFilledScore => MaxFillModifier + GameConfig.DefaultMaxSandFillCount;
		public int MaxFillModifier { get; private set; } = 0;
		public int CurrentSandCount { get; private set; } = 0;

		public ResourcesData(IResource<int> softCurrency, int startCount)
		{
			SoftCurrency = softCurrency;	
			SoftCurrency.Count = startCount;	
		}
		
		public void AddSand(int count) =>
			CurrentSandCount += count;

		public void DecreaseSand(int count) =>
			CurrentSandCount -= count;

		public void AddMoney(int count) =>
			SoftCurrency.Count += count;

		public void DecreaseMoney(int count) =>
			SoftCurrency.Count -= count;
	}
}