using System;

namespace Sources.Presentation.SceneEntity
{
	public interface ICollectable
	{
		event Action Collected;
	}
}