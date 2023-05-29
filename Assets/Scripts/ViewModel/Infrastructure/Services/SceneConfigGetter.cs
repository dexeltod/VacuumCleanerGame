using Cysharp.Threading.Tasks;
using Model;
using Model.Configs;
using Model.DI;

namespace ViewModel.Infrastructure.Services
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