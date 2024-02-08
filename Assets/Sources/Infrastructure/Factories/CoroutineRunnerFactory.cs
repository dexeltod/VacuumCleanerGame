using System;
using Sources.Infrastructure.Factories.Coroutine;
using Sources.ServicesInterfaces;
using Sources.UseCases.Scene;
using Sources.Utils.Configs.Scripts;
using VContainer;

namespace Sources.Infrastructure.Factories
{
	public class CoroutineRunnerFactory
	{
		private readonly IAssetFactory _assetFactory;

		private string CoroutineRunnerPath => ResourcesAssetPath.GameObjects.CoroutineRunner;

		[Inject]
		public CoroutineRunnerFactory(IAssetFactory assetFactory) =>
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));

		public ICoroutineRunner Create() =>
			_assetFactory.InstantiateAndGetComponent<CoroutineRunner>(CoroutineRunnerPath);
	}
}