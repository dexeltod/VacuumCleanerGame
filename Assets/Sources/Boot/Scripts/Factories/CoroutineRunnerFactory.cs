using System;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Infrastructure.CoroutineRunner;
using Sources.Utils;
using VContainer;

namespace Sources.Boot.Scripts.Factories
{
	public class CoroutineRunnerFactory
	{
		private readonly IAssetLoader _assetLoader;

		[Inject]
		public CoroutineRunnerFactory(IAssetLoader assetLoader) =>
			_assetLoader
				= assetLoader ?? throw new ArgumentNullException(nameof(assetLoader));

		private string CoroutineRunnerPath => ResourcesAssetPath.GameObjects.CoroutineRunner;

		public ICoroutineRunner Create() =>
			_assetLoader.InstantiateAndGetComponent<CoroutineRunner>(CoroutineRunnerPath);
	}
}