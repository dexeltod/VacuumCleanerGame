using System;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Presentation.SceneEntity;
using Sources.PresentationInterfaces;
using Sources.Utils;

namespace Sources.Boot.Scripts.Factories.Presentation.Scene
{
	public class LoadingCurtainFactory
	{
		private readonly IAssetLoader _loader;

		public LoadingCurtainFactory(IAssetLoader loader) =>
			_loader = loader ?? throw new ArgumentNullException(nameof(loader));

		public ILoadingCurtain Create() =>
			_loader.InstantiateAndGetComponent<LoadingCurtain>(ResourcesAssetPath.Scene.UIResources.LoadingCurtain);
	}
}