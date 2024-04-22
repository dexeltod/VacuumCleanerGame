using System;
using Sources.Controllers;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Common.Factory.Decorators;
using Sources.InfrastructureInterfaces.Providers;
using Sources.Services;
using VContainer;

namespace Sources.Infrastructure.Factories.Presenters
{
	public class ResourcesProgressPresenterFactory : PresenterFactory<ResourcesProgressPresenter>
	{
		private readonly IGameplayInterfacePresenterProvider _gameplayInterfacePresenterProvider;
		private readonly IPersistentProgressServiceProvider _persistentProgressService;
		private readonly IFillMeshShaderControllerProvider _fillMeshShaderControllerProvider;
		private readonly ISandParticleSystemProvider _sandParticleSystemProvider;
		private readonly ICoroutineRunnerProvider _coroutineRunnerProvider;

		private IResourceModelReadOnly ResourceModelReadOnly =>
			_persistentProgressService.Implementation.GlobalProgress.ResourceModelReadOnly;

		[Inject]
		public ResourcesProgressPresenterFactory(
			IGameplayInterfacePresenterProvider gameplayInterfacePresenterProvider,
			IPersistentProgressServiceProvider persistentProgressService,
			IFillMeshShaderControllerProvider fillMeshShaderControllerProvider,
			ISandParticleSystemProvider sandParticleSystemProvider,
			ICoroutineRunnerProvider coroutineRunnerProvider
		)
		{
			_gameplayInterfacePresenterProvider = gameplayInterfacePresenterProvider ??
				throw new ArgumentNullException(nameof(gameplayInterfacePresenterProvider));
			_persistentProgressService = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));
			_fillMeshShaderControllerProvider = fillMeshShaderControllerProvider ??
				throw new ArgumentNullException(nameof(fillMeshShaderControllerProvider));
			_sandParticleSystemProvider = sandParticleSystemProvider ??
				throw new ArgumentNullException(nameof(sandParticleSystemProvider));
			_coroutineRunnerProvider = coroutineRunnerProvider ??
				throw new ArgumentNullException(nameof(coroutineRunnerProvider));
		}

		public override ResourcesProgressPresenter Create()
		{
			var sandParticlePlayerSystem
				= new SandParticlePlayerSystem(
					_sandParticleSystemProvider,
					_coroutineRunnerProvider,
					playTime: 1
				);

			return new ResourcesProgressPresenter(
				_gameplayInterfacePresenterProvider,
				ResourceModelReadOnly,
				ResourceModelReadOnly as IResourceModelModifiable,
				_fillMeshShaderControllerProvider,
				sandParticlePlayerSystem
			);
		}
	}
}