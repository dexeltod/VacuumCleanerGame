using Sources.Infrastructure.Factories.Coroutine;
using Sources.ServicesInterfaces;
using Sources.UseCases.Scene;
using Sources.Utils.Configs.Scripts;

namespace Sources.Infrastructure.Factories
{
	public class CoroutineRunnerFactory
	{
		private readonly IAssetProvider _assetProvider;

		public CoroutineRunnerFactory(IAssetProvider assetProvider)
		{
			_assetProvider = assetProvider;
		}

		public ICoroutineRunner Create()
		{
			return _assetProvider.InstantiateAndGetComponent<CoroutineRunner>(
				ResourcesAssetPath.GameObjects.CoroutineRunner
			);
		}
	}
}