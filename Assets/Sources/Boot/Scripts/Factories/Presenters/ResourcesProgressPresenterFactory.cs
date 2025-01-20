using System;
using Sources.BusinessLogic.Repository;
using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Services;
using Sources.PresentationInterfaces.Player;
using Sources.Utils;
using Sources.Utils.Enums;

namespace Sources.Boot.Scripts.Factories.Presenters
{
	public class ResourcesProgressPresenterFactory : IResourcesProgressPresenterFactory
	{
		private readonly IGameplayInterfacePresenter _gameplayInterfacePresenterProvider;
		private readonly IPersistentProgressService _persistentProgressService;
		private readonly IFillMeshShaderController _fillMeshShaderControllerProvider;
		private readonly ISandParticleView _sandParticleView;
		private readonly ICoroutineRunner _coroutineRunnerProvider;
		private readonly IPlayerModelRepository _playerModelRepositoryProvider;

		private IResourceModelReadOnly ResourceModelReadOnly =>
			_persistentProgressService.GlobalProgress.ResourceModelReadOnly;

		public ResourcesProgressPresenterFactory(
			IGameplayInterfacePresenter gameplayInterfacePresenterProvider,
			IPersistentProgressService persistentProgressService,
			IFillMeshShaderController fillMeshShaderControllerProvider,
			ISandParticleView sandParticleViewProvider,
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
			_sandParticleView = sandParticleViewProvider ??
			                    throw new ArgumentNullException(nameof(sandParticleViewProvider));
			_coroutineRunnerProvider = coroutineRunnerProvider ??
			                           throw new ArgumentNullException(nameof(coroutineRunnerProvider));
			_playerModelRepositoryProvider = playerModelRepositoryProvider ??
			                                 throw new ArgumentNullException(nameof(playerModelRepositoryProvider));
		}

		public IResourcesProgressPresenter Create() =>
			new ResourcesProgressPresenter(
				_gameplayInterfacePresenterProvider,
				ResourceModelReadOnly,
				ResourceModelReadOnly as IResourceModel,
				_fillMeshShaderControllerProvider,
				new SandParticlePlayerSystem(
					_sandParticleView,
					_coroutineRunnerProvider,
					playTime: 1
				),
				_playerModelRepositoryProvider.Get(ProgressType.MaxCashScore)
			);
	}
}
