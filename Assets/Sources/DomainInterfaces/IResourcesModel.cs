using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.DomainInterfaces
{
	public interface IResourcesModel
	{
		IResource<int> SoftCurrency { get; }
		IResource<int> HardCurrency { get; }
		IResource<int> Score        { get; }

		int  MaxScore    { get; }
		int  MaxModifier { get; }
		void AddSand(int newValue);

		int  CurrentSandCount { get; }
		int  GlobalSandCount  { get; }
		void DecreaseSand(int  newValue);
		void AddMoney(int      newValue);
		void DecreaseMoney(int newValue);
	}
}