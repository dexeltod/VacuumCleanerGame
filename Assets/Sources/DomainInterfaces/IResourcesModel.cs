using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.DomainInterfaces
{
	public interface IResourcesModel
	{
		IResource<int> SoftCurrency { get; }
		IResource<int> HardCurrency { get; }
		IResource<int> Score        { get; }

		int  MaxFilledScore   { get; }
		int  MaxFillModifier  { get; }
		int  CurrentSandCount { get; }
		int  GlobalSandCount  { get; }
		void AddSand(int       count);
		void DecreaseSand(int  count);
		void AddMoney(int      count);
		void DecreaseMoney(int count);
	}
}