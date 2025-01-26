using System;

namespace Sources.PresentationInterfaces
{
	public interface ICollideable
	{
		event Action Collided;
	}
}