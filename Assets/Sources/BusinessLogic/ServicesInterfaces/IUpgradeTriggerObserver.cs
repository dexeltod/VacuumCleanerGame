using System;

namespace Sources.BuisenessLogic.ServicesInterfaces
{
	public interface IUpgradeTriggerObserver
	{
		event Action<bool> TriggerEntered;
	}
}