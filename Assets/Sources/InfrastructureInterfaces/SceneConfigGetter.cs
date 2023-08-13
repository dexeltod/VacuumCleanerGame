using Sources.Application.Utils.Configs;
using Sources.DIService;
using Sources.ServicesInterfaces;

namespace Sources.InfrastructureInterfaces
{
	public class SceneConfigGetter : ISceneConfigGetter
	{
		private readonly IResourceProvider _assetProvider;
		private string _sceneName;
		private SceneConfig _lastSceneConfig;

		public SceneConfigGetter()
		{
			_assetProvider = GameServices.Container.Get<IResourceProvider>();
		}

		public SceneConfig GetSceneConfig(string sceneConfigName)
		{
			if (sceneConfigName == _sceneName)
				return _lastSceneConfig;

			_lastSceneConfig = _assetProvider.Load<SceneConfig>(sceneConfigName);
			return _lastSceneConfig;
		}
	}
}