using System;

namespace Model.Infrastructure.Services
{
	public interface ISceneLoadInformer : IService
	{
		event Action SceneLoaded;
	}
}