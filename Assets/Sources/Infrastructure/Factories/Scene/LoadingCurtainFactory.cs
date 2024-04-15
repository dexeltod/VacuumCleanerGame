using System;
using Sources.Infrastructure.Configs.Scripts;
using Sources.Presentation.SceneEntity;
using Sources.ServicesInterfaces;
using Sources.Utils;
using VContainer;

namespace Sources.Infrastructure.Factories.Scene
{
	public class LoadingCurtainFactory
	{
		private readonly IAssetFactory _factory;

		[Inject]
		public LoadingCurtainFactory(IAssetFactory factory) =>
			_factory = factory ?? throw new ArgumentNullException(nameof(factory));

		public LoadingCurtain Create() =>
			_factory.InstantiateAndGetComponent<LoadingCurtain>(ResourcesAssetPath.Scene.UIResources.LoadingCurtain);
	}
}