using Sources.Utils;

namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface ICurrencyReadOnlyProgress<out T> : IReadOnlyProgress<T>
	{
		CurrencyResourceType ResourceType { get; }
	}
}