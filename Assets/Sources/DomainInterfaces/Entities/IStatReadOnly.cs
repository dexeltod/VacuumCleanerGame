using System;

namespace Sources.Domain.Temp
{
	public interface IStatReadOnly
	{
		event Action Changed;
		float Value { get; }
		int Id { get; }
	}
}