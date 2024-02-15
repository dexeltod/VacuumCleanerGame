using System;
using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Common.Factory;
using Sources.Infrastructure.Common.Factory.Decorators;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces.Providers;
using Sources.Presentation.UI;
using Sources.PresentationInterfaces;
using VContainer;

namespace Sources.Infrastructure.Factories.Presenters
{
	public class ResourcesProgressPresenterFactory : PresenterFactory<ResourcesProgressPresenter>
	{
		private readonly IGameplayInterfaceProvider _gameplayInterfaceProvider;
		private readonly IPersistentProgressServiceProvider _persistentProgressService;
		private readonly IFillMeshShaderControllerProvider _fillMeshShaderControllerProvider;
		private readonly ISandParticleSystemProvider _sandParticleSystemProvider;
		private readonly ICoroutineRunnerProvider _coroutineRunnerProvider;

		private IResourcesModel Resources => _persistentProgressService.Implementation.GameProgress.ResourcesModel;

		[Inject]
		public ResourcesProgressPresenterFactory(
			IGameplayInterfaceProvider gameplayInterfaceProvider,
			IPersistentProgressServiceProvider persistentProgressService,
			IFillMeshShaderControllerProvider fillMeshShaderControllerProvider,
			ISandParticleSystemProvider sandParticleSystemProvider,
			ICoroutineRunnerProvider coroutineRunnerProvider
		)
		{
			_gameplayInterfaceProvider = gameplayInterfaceProvider ??
				throw new ArgumentNullException(nameof(gameplayInterfaceProvider));
			_persistentProgressService = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));
			_fillMeshShaderControllerProvider = fillMeshShaderControllerProvider ??
				throw new ArgumentNullException(nameof(fillMeshShaderControllerProvider));
			_sandParticleSystemProvider = sandParticleSystemProvider ??
				throw new ArgumentNullException(nameof(sandParticleSystemProvider));
			_coroutineRunnerProvider = coroutineRunnerProvider ??
				throw new ArgumentNullException(nameof(coroutineRunnerProvider));
		}

		public override ResourcesProgressPresenter Create() =>
			new ResourcesProgressPresenter(
				_gameplayInterfaceProvider,
				Resources,
				_fillMeshShaderControllerProvider,
				_sandParticleSystemProvider.Implementation,
				_coroutineRunnerProvider.Implementation
			);
	}
}