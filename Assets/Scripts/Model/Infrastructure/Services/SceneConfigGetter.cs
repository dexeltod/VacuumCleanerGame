using Cysharp.Threading.Tasks;
using Model.DI;
using Model.Infrastructure.Services;

namespace Model
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