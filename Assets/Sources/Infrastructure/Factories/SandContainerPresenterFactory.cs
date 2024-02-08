using System;
using Sources.Controllers;
using Sources.DomainInterfaces;
using Sources.ServicesInterfaces;

namespace Sources.Infrastructure.Factories
{
	public class SandContainerPresenterFactory
	{
		private readonly IResourcesModel _gameProgressResourcesModel;
		private readonly ISandContainerView _sandContainerView;
		private readonly IResourceProgressEventHandler _resourcesProgress;
		private readonly ISandParticleSystem _particleSystem;
		private readonly CoroutineRunnerFactory _coroutineRunnerFactory;

		public SandContainerPresenterFactory(
			IResourcesModel gameProgressResourcesModel,
			ISandContainerView sandContainerView,
			IResourceProgressEventHandler resourcesProgress,
			ISandParticleSystem particleSystem,
			CoroutineRunnerFactory coroutineRunnerFactory
		
		)
		{
			_gameProgressResourcesModel = gameProgressResourcesModel ??
				throw new ArgumentNullException(nameof(gameProgressResourcesModel));
			_sandContainerView = sandContainerView ?? throw new ArgumentNullException(nameof(sandContainerView));
			_resourcesProgress = resourcesProgress ?? throw new ArgumentNullException(nameof(resourcesProgress));
			_particleSystem = particleSystem ?? throw new ArgumentNullException(nameof(particleSystem));
			_coroutineRunnerFactory = coroutineRunnerFactory ??
				throw new ArgumentNullException(nameof(coroutineRunnerFactory));
		}

		public SandContainerPresenter Create()
		{
			var coroutineRunner = _coroutineRunnerFactory.Create();

			return new SandContainerPresenter(
				_gameProgressResourcesModel,
				_sandContainerView,
				_resourcesProgress,
				_particleSystem,
				coroutineRunner
			);
		}
	}
}