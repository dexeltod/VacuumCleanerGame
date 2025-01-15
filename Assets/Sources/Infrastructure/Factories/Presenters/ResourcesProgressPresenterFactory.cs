using System;
using Sources.BuisenessLogic.Repository;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Services;
using Sources.Infrastructure.Services.DomainServices;
using Sources.PresentationInterfaces.Player;
using Sources.Utils;
using Sources.Utils.Enums;
using VContainer;

namespace Sources.Infrastructure.Factories.Presenters
{
	public class ResourcesProgressPresenterFactory
	{
		private readonly IGameplayInterfacePresenter _gameplayInterfacePresenterProvider;
		private readonly PersistentProgressService _persistentProgressService;
		private readonly IFillMeshShaderController _fillMeshShaderControllerProvider;
		private readonly ISandParticleSystem _sandParticleSystemProvider;
		private readonly ICoroutineRunner _coroutineRunnerProvider;
		private readonly IPlayerModelRepository _playerModelRepositoryProvider;

		private IResourceModelReadOnly ResourceModelReadOnly =>
			_persistentProgressService.GlobalProgress.ResourceModelReadOnly;

		[Inject]
		public ResourcesProgressPresenterFactory(
			IGameplayInterfacePresenter gameplayInterfacePresenterProvider,
			PersistentProgressService persistentProgressService,
			IFillMeshShaderController fillMeshShaderControllerProvider,
			ISandParticleSystem sandParticleSystemProvider,
			ICoroutineRunner coroutineRunnerProvider,
			IPlayerModelRepository playerModelRepositoryProvider
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
			_playerModelRepositoryProvider = playerModelRepositoryProvider ??
			                                 throw new ArgumentNullException(nameof(playerModelRepositoryProvider));
		}

		public ResourcesProgressPresenter Create()
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
				ResourceModelReadOnly as IResourceModel,
				_fillMeshShaderControllerProvider,
				sandParticlePlayerSystem,
				_playerModelRepositoryProvider.Get(ProgressType.MaxCashScore)
			);
		}
	}
}