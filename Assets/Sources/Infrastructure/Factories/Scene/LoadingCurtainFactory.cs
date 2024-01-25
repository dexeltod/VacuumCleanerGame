using System;
using Sources.Presentation.SceneEntity;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;
using VContainer;

namespace Sources.Application
{
	public class LoadingCurtainFactory
	{
		private readonly IAssetProvider _provider;

		[Inject]
		public LoadingCurtainFactory(IAssetProvider provider) =>
			_provider = provider ?? throw new ArgumentNullException(nameof(provider));

		public LoadingCurtain Create() =>
			_provider.InstantiateAndGetComponent<LoadingCurtain>(ResourcesAssetPath.Scene.UIResources.LoadingCurtain);
	}
}