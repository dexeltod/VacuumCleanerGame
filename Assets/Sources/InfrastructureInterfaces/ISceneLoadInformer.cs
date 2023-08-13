using System;
using Sources.DIService;

namespace Sources.InfrastructureInterfaces
{
	public interface ISceneLoadInformer : IService
	{
		event Action SceneLoaded;
	}
}