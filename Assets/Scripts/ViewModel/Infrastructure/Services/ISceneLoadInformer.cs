using System;

namespace ViewModel.Infrastructure.Services
{
	public interface ISceneLoadInformer : IService
	{
		event Action SceneLoaded;
	}
}