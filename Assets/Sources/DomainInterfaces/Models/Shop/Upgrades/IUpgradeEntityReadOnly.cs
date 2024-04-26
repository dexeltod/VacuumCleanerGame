using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.Domain.Temp
{
	public interface IUpgradeEntityReadOnly
	{
		int ConfigId { get; }
		IReadOnlyProgressValue<int> CurrentLevel { get; }
		int Value { get; }
	}
}