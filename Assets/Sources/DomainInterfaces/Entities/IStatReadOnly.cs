using System;

namespace Sources.DomainInterfaces.Entities
{
	public interface IStatReadOnly
	{
		float Value { get; }
		int Id { get; }
		event Action Changed;
	}
}