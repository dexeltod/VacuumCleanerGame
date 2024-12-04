using System;

namespace Sources.DomainInterfaces.Entities
{
	public interface IStatReadOnly
	{
		event Action Changed;
		float Value { get; }
		int Id { get; }
	}
}