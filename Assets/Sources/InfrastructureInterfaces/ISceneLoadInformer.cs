using System;

namespace InfrastructureInterfaces
{
	public interface ISceneLoadInformer : IService
	{
		event Action SceneLoaded;
	}
}