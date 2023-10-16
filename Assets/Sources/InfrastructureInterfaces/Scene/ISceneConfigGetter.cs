using Sources.DIService;
using Sources.Utils.Configs;

namespace Sources.InfrastructureInterfaces.Scene
{
	public interface ISceneConfigGetter : IService
	{
		SceneConfig Get(string sceneConfigName);
	}
}