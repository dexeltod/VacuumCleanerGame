using Cysharp.Threading.Tasks;
using Model;
using Model.Configs;

namespace ViewModel.Infrastructure.Services
{
	public interface ISceneConfigGetter : IService
	{
		UniTask<SceneConfig> GetSceneConfig(string sceneConfigName);
	}
}