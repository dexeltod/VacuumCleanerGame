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
		private readonly IAssetResolver _assetResolver;

		private string CoroutineRunnerPath => ResourcesAssetPath.GameObjects.CoroutineRunner;

		[Inject]
		public CoroutineRunnerFactory(IAssetResolver assetResolver) =>
			_assetResolver = assetResolver ?? throw new ArgumentNullException(nameof(assetResolver));

		public ICoroutineRunner Create() =>
			_assetResolver.InstantiateAndGetComponent<CoroutineRunner>(CoroutineRunnerPath);
	}
}