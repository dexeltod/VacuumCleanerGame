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

		public LoadingCurtain Load() =>
			_provider.InstantiateAndGetComponent<LoadingCurtain>(ResourcesAssetPath.GameObjects.LoadinCrutain);
	}
}