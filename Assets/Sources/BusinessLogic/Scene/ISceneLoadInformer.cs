using System;

namespace Sources.BusinessLogic.Scene
{
	public interface ISceneLoadInformer
	{
		bool IsSceneLoaded { get; }
		event Action SceneLoaded;
	}
}