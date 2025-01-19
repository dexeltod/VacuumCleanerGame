using System;

namespace Sources.PresentationInterfaces
{
	public interface IUpgradeTriggerObserver
	{
		event Action<bool> TriggerEntered;
	}
}