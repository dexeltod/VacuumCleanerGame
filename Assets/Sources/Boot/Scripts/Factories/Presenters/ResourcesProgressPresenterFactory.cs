using System;
using Sources.BusinessLogic.Repository;
using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Services;
using Sources.PresentationInterfaces;
using Sources.PresentationInterfaces.Player;
using Sources.PresentationInterfaces.Triggers;
using Sources.Utils;
using Sources.Utils.Enums;

namespace Sources.Boot.Scripts.Factories.Presenters
{
	public class ResourcesProgressPresenterFactory : IResourcesProgressPresenterFactory
	{
		private readonly ICoroutineRunner _coroutineRunner;
		private readonly IFillMeshShader _fillMeshShaderProvider;
		private readonly IGameplayInterfaceView _gameplayInterfacePresenter;
		private readonly IPersistentProgressService _persistentProgressService;
		private readonly IPlayerModelRepository _playerModelRepositoryProvider;
		private readonly ISandParticleView _sandParticleView;
		private readonly ITriggerSell _triggerSell;

		public ResourcesProgressPresenterFactory(IGameplayInterfaceView gameplayInterfacePresenter,
			IPersistentProgressService persistentProgressService,
			IFillMeshShader fillMeshShaderProvider,
			ISandParticleView sandParticleViewProvider,
			ICoroutineRunner coroutineRunnerProvider,
			IPlayerModelRepository playerModelRepositoryProvider,
			ITriggerSell triggerSell)
		{
			_gameplayInterfacePresenter = gameplayInterfacePresenter ??
			                              throw new ArgumentNullException(nameof(gameplayInterfacePresenter));
			_persistentProgressService = persistentProgressService ??
			                             throw new ArgumentNullException(nameof(persistentProgressService));
			_fillMeshShaderProvider = fillMeshShaderProvider ??
			                          throw new ArgumentNullException(nameof(fillMeshShaderProvider));
			_sandParticleView = sandParticleViewProvider ??
			                    throw new ArgumentNullException(nameof(sandParticleViewProvider));
			_coroutineRunner = coroutineRunnerProvider ??
			                   throw new ArgumentNullException(nameof(coroutineRunnerProvider));
			_playerModelRepositoryProvider = playerModelRepositoryProvider ??
			                                 throw new ArgumentNullException(nameof(playerModelRepositoryProvider));
			_triggerSell = triggerSell ?? throw new ArgumentNullException(nameof(triggerSell));
		}

		private IResourceModelReadOnly ResourceModelReadOnly =>
			_persistentProgressService.GlobalProgress.ResourceModelReadOnly;

		public IResourcesProgressPresenter Create() =>
			new ResourcesProgressPresenter(
				_gameplayInterfacePresenter,
				ResourceModelReadOnly,
				ResourceModelReadOnly as IResourceModel,
				_fillMeshShaderProvider,
				new SandParticlePlayerSystem(
					_sandParticleView,
					_coroutineRunner,
					1
				),
				_playerModelRepositoryProvider.Get(ProgressType.MaxCashScore),
				_triggerSell,
				_coroutineRunner
			);
	}
}
