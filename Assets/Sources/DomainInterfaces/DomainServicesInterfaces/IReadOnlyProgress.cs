using System;

namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface IReadOnlyProgress<out T>
	{
		int Id { get; }
		T ReadOnlyValue { get; }
		T ReadOnlyMaxValue { get; }
		string Name { get; }
		event Action Changed;
		event Action HalfReached;
	}
}