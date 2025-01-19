using System;
using Sources.Infrastructure.Services;
using Sources.Presentation.SceneEntity;
using Sources.PresentationInterfaces;
using Sources.Utils;
using VContainer;

namespace Sources.Boot.Scripts.Factories.Presentation.Scene
{
	public class LoadingCurtainFactory
	{
		private readonly AssetFactory _factory;

		[Inject]
		public LoadingCurtainFactory(AssetFactory factory) =>
			_factory = factory ?? throw new ArgumentNullException(nameof(factory));

		public ILoadingCurtain Create() =>
			_factory.InstantiateAndGetComponent<LoadingCurtain>(ResourcesAssetPath.Scene.UIResources.LoadingCurtain);
	}
}