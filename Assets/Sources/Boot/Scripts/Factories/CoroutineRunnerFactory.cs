using System;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Infrastructure.CoroutineRunner;
using Sources.Utils;
using VContainer;

namespace Sources.Boot.Scripts.Factories
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
			_assetFactory.InstantiateAndGetComponent<CoroutineRunner>(CoroutineRunnerPath);
	}
}