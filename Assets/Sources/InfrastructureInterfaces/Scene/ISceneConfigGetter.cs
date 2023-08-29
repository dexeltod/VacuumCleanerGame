using Sources.Application.Utils.Configs;
using Sources.DIService;

namespace Sources.InfrastructureInterfaces.Scene
{
	public interface ISceneConfigGetter : IService
	{
		SceneConfig GetSceneConfig(string sceneConfigName);
	}
}