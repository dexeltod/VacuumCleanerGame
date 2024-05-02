using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.DomainInterfaces.Models.Shop.Upgrades
{
	public interface IUpgradeEntityReadOnly
	{
		int ConfigId { get; }
		IReadOnlyProgressValue<int> CurrentLevel { get; }
		int Value { get; }
	}
}