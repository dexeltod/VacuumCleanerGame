using Cysharp.Threading.Tasks;
using Sources.Core.DI;
using Sources.Core.Utils.Configs;
using Sources.Infrastructure.Services.Interfaces;

namespace Sources.Infrastructure.InfrastructureInterfaces
{
	public class SceneConfigGetter : ISceneConfigGetter
	{
		private readonly IAssetProvider _assetProvider;

		public SceneConfigGetter()
		{
			_assetProvider = ServiceLocator.Container.Get<IAssetProvider>();
		}

		public async UniTask<SceneConfig> GetSceneConfig(string sceneConfigName) => 
			await _assetProvider.LoadAsync<SceneConfig>(sceneConfigName);
	}
}