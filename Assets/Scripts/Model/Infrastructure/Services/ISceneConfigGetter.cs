using Cysharp.Threading.Tasks;

namespace Model.Infrastructure.Services
{
	public interface ISceneConfigGetter : IService
	{
		UniTask<SceneConfig> GetSceneConfig(string sceneConfigName);
	}
}