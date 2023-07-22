using Sources.Core.Domain.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.Core.Domain.DomainInterfaces
{
	public interface IResourcesData
	{
		IResource<int> SoftCurrency { get; }
		int MaxFilledScore { get; }
		int MaxFillModifier { get; }
		int CurrentSandCount { get; }
		void AddSand(int count);
		void DecreaseSand(int count);
		void AddMoney(int count);
		void DecreaseMoney(int count);
	}
}