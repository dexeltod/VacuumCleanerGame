using System;
using Sources.DomainInterfaces.ViewEntities;

namespace Sources.Domain.ViewEntity
{
	public class ViewEntity : IViewEntity
	{
		public event Action Enabled;
		public event Action Disabled;

		public void SetActive(bool isEnabled)
		{
			if (isEnabled)
				Enabled?.Invoke();
			else
				Disabled?.Invoke();
		}
	}
}
