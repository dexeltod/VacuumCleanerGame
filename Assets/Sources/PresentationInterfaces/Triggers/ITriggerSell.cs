using System;

namespace Sources.PresentationInterfaces.Triggers
{
	public interface ITriggerSell
	{
		event Action<bool> OnTriggerStayed;
	}
}