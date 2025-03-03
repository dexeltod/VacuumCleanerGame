using System;

namespace Sources.DomainInterfaces.ViewEntities
{
	public interface IViewEvent
	{
		event Action Enabled;
		event Action Disabled;
	}
}
