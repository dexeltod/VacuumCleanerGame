using Sources.DIService;
using Sources.Utils.Configs;

namespace Sources.InfrastructureInterfaces.Scene
{
	public interface ISceneConfigGetter : IService
	{
		SceneConfig GetSceneConfig(string sceneConfigName);
	}
}