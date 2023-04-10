using Cysharp.Threading.Tasks;

namespace Model
{
	public interface ISceneConfigGetter : IService
	{
		UniTask<SceneConfig> GetSceneConfig(string sceneConfigName);
	}
}