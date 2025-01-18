using System;

namespace Sources.BuisenessLogic.Scene
{
	public interface ISceneLoadInformer
	{
		event Action SceneLoaded;
		bool IsSceneLoaded { get; }
	}
}