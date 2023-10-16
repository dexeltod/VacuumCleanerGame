using Sources.DIService;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs;

namespace Sources.InfrastructureInterfaces.Scene
{
	public class SceneConfigGetter : ISceneConfigGetter
	{
		private readonly IAssetProvider _assetProvider;
		private string _sceneName;
		private SceneConfig _lastSceneConfig;

		public SceneConfigGetter()
		{
			_assetProvider = GameServices.Container.Get<IAssetProvider>();
		}

		public SceneConfig Get(string sceneConfigName)
		{
			if (sceneConfigName == _sceneName)
				return _lastSceneConfig;

			_lastSceneConfig = _assetProvider.Load<SceneConfig>(sceneConfigName);
			return _lastSceneConfig;
		}
	}
}