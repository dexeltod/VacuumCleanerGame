using Sources.Utils;

namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface ICurrencyReadOnlyProgress<out T> : IReadOnlyProgress<T>
	{
		ResourceType ResourceType { get; }
	}
}
