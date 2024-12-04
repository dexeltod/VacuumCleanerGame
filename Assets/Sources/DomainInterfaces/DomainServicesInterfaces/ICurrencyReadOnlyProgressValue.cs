using Sources.Utils;

namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface ICurrencyReadOnlyProgressValue<out T> : IReadOnlyProgressValue<T>
	{
		CurrencyResourceType ResourceType { get; }
	}
}