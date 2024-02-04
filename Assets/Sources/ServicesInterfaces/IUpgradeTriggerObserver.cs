using System;

namespace Sources.ServicesInterfaces
{
	public interface IUpgradeTriggerObserver
	{
		event Action<bool> TriggerEntered;
	}
}