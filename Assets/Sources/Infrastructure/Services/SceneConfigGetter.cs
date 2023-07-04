using Application.Configs;
using Application.DI;
using Cysharp.Threading.Tasks;
using InfrastructureInterfaces;

namespace Infrastructure.Services
{
	public class SceneConfigGetter : ISceneConfigGetter
	{
		private readonly IAssetProvider _assetProvider;

		public SceneConfigGetter()
		{
			_assetProvider = ServiceLocator.Container.GetSingle<IAssetProvider>();
		}

		public async UniTask<SceneConfig> GetSceneConfig(string sceneConfigName) => 
			await _assetProvider.LoadAsync<SceneConfig>(sceneConfigName);
	}
}