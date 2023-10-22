using System;
using Sources.DIService;

namespace Sources.InfrastructureInterfaces.Scene
{
	public interface ISceneLoadInformer : IService
	{
		event Action SceneLoaded;
		bool         IsSceneLoaded { get; }
	}
}