using Application.Configs;
using Cysharp.Threading.Tasks;

namespace InfrastructureInterfaces
{
	public interface ISceneConfigGetter : IService
	{
		UniTask<SceneConfig> GetSceneConfig(string sceneConfigName);
	}
}