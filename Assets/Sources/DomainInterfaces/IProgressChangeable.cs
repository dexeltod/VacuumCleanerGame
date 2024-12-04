using System;

namespace Sources.DomainInterfaces
{
	public interface IProgressChangeable
	{
		event Action Changed;
	}
}