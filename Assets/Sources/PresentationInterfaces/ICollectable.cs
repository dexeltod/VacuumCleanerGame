using System;

namespace Sources.PresentationInterfaces
{
	public interface ICollectable
	{
		event Action Collected;
	}
}