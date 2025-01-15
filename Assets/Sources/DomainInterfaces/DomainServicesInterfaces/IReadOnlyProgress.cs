using System;

namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface IReadOnlyProgress<out T>
	{
		int Id { get; }
		T Value { get; }
		string Name { get; }
		event Action Changed;
	}
}
