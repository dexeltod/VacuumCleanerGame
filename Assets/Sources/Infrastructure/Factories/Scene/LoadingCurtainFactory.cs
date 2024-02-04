using System;
using Sources.Presentation.SceneEntity;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;
using VContainer;

namespace Sources.Infrastructure.Factories.Scene
{
	public class LoadingCurtainFactory
	{
		private readonly IAssetResolver _resolver;

		[Inject]
		public LoadingCurtainFactory(IAssetResolver resolver) =>
			_resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));

		public LoadingCurtain Create() =>
			_resolver.InstantiateAndGetComponent<LoadingCurtain>(ResourcesAssetPath.Scene.UIResources.LoadingCurtain);
	}
}