using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.DomainInterfaces.Models.Shop.Upgrades
{
	public interface IStatUpgradeEntityReadOnly
	{
		int ConfigId { get; }
		IReadOnlyProgress<int> LevelProgress { get; }
		int Value { get; }
	}
}