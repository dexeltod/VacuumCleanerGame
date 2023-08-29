using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.DomainInterfaces
{
	public interface IResourcesData
	{
		IResource<int> SoftCurrency { get; }
		int MaxFilledScore { get; }
		int MaxFillModifier { get; }
		int CurrentSandCount { get; }
		IResource<int> HardCurrency { get; }
		void AddSand(int count);
		void DecreaseSand(int count);
		void AddMoney(int count);
		void DecreaseMoney(int count);
	}
}