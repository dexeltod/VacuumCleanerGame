using System;

namespace Sources.BusinessLogic.ServicesInterfaces
{
	public interface IUpgradeTriggerObserver
	{
		event Action<bool> TriggerEntered;
	}
}
