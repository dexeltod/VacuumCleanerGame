using System;

namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface IEventProgress
	{
		event Action Changed;
		event Action HalfReached;
	}
}