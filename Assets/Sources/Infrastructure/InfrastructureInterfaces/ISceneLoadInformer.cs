using System;
using Sources.Core;

namespace Sources.Infrastructure.InfrastructureInterfaces
{
	public interface ISceneLoadInformer : IService
	{
		event Action SceneLoaded;
	}
}