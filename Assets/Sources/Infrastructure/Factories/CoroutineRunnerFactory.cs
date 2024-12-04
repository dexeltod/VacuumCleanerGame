using System;
using Sources.ServicesInterfaces;
using Sources.Utils;
using VContainer;

namespace Sources.Infrastructure.Factories
{
	public class CoroutineRunnerFactory
	{
		private readonly IAssetFactory _assetFactory;

		[Inject]
		public CoroutineRunnerFactory(IAssetFactory assetFactory) =>
			_assetFactory
				= assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));

		private string CoroutineRunnerPath => ResourcesAssetPath.GameObjects.CoroutineRunner;

		public ICoroutineRunner Create() =>
			_assetFactory.InstantiateAndGetComponent<CoroutineRunner.CoroutineRunner>(CoroutineRunnerPath);
	}
}