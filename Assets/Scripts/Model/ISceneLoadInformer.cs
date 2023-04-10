using System;

namespace Model
{
	public interface ISceneLoadInformer : IService
	{
		event Action SceneLoaded;
	}
}