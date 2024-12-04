using System;

namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface IReadOnlyProgressValue<out T>
	{
		event Action Changed;
		T Value { get; }
	}
}