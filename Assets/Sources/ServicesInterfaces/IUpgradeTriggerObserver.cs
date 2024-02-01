using System;

namespace Sources.Presentation
{
	public interface IUpgradeTriggerObserver
	{
		event Action<bool> TriggerEntered;
	}
}