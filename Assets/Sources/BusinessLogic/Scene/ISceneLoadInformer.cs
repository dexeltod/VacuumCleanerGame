using System;

namespace Sources.BusinessLogic.Scene
{
	public interface ISceneLoadInformer
	{
		event Action SceneLoaded;
		bool IsSceneLoaded { get; }
	}
}
