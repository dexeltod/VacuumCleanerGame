using Cysharp.Threading.Tasks;
using Sources.Core;
using Sources.Core.Utils.Configs;

namespace Sources.Infrastructure.InfrastructureInterfaces
{
	public interface ISceneConfigGetter : IService
	{
		UniTask<SceneConfig> GetSceneConfig(string sceneConfigName);
	}
}