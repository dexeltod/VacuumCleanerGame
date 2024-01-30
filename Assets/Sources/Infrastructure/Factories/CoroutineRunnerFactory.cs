using Sources.Infrastructure.Factories.Coroutine;
using Sources.ServicesInterfaces;
using Sources.UseCases.Scene;
using Sources.Utils.Configs.Scripts;
using VContainer;

namespace Sources.Infrastructure.Factories
{
	public class CoroutineRunnerFactory
	{
		private readonly IAssetProvider _assetProvider;
		private ICoroutineRunner _coroutineRunner;

		private string GameObjectsCoroutineRunner => ResourcesAssetPath.GameObjects.CoroutineRunner;

		[Inject]
		public CoroutineRunnerFactory(IAssetProvider assetProvider) =>
			_assetProvider = assetProvider;

		public ICoroutineRunner Create()
		{
			_coroutineRunner ??= _assetProvider.InstantiateAndGetComponent<CoroutineRunner>(GameObjectsCoroutineRunner);
			return _coroutineRunner;
		}
	}
}