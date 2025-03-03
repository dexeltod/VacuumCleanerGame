using System;
using System.Collections.Generic;
using Sources.BusinessLogic.Repository;
using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Services;
using Sources.PresentationInterfaces.Common;
using Sources.PresentationInterfaces.Player;
using Sources.PresentationInterfaces.Triggers;
using Sources.Utils;
using Sources.Utils.Enums;

namespace Sources.Boot.Scripts.Factories.Presenters
{
	public class ResourcesProgressPresenterFactory
	{
		private readonly ICoroutineRunner _coroutineRunner;
		private readonly IFillMeshShader _fillMeshShaderProvider;
		private readonly IPersistentProgressService _persistentProgressService;
		private readonly IPlayerModelRepository _playerModelRepositoryProvider;
		private readonly Dictionary<int, IResourcePresentation> _rocks;
		private readonly ISandParticleView _sandParticleView;
		private readonly SceneResourcesRepository _sceneResourcesRepository;
		private readonly ITriggerSell _triggerSell;

		public ResourcesProgressPresenterFactory(
			IPersistentProgressService persistentProgressService,
			IFillMeshShader fillMeshShaderProvider,
			ISandParticleView sandParticleViewProvider,
			ICoroutineRunner coroutineRunnerProvider,
			IPlayerModelRepository playerModelRepositoryProvider,
			ITriggerSell triggerSell,
			Dictionary<int, IResourcePresentation> rocks,
			SceneResourcesRepository sceneResourcesRepository)
		{
			_persistentProgressService =
				persistentProgressService ?? throw new ArgumentNullException(nameof(persistentProgressService));
			_fillMeshShaderProvider = fillMeshShaderProvider ?? throw new ArgumentNullException(nameof(fillMeshShaderProvider));
			_sandParticleView = sandParticleViewProvider ?? throw new ArgumentNullException(nameof(sandParticleViewProvider));
			_coroutineRunner = coroutineRunnerProvider ?? throw new ArgumentNullException(nameof(coroutineRunnerProvider));
			_playerModelRepositoryProvider = playerModelRepositoryProvider
			                                 ?? throw new ArgumentNullException(nameof(playerModelRepositoryProvider));
			_triggerSell = triggerSell ?? throw new ArgumentNullException(nameof(triggerSell));
			_rocks = rocks ?? throw new ArgumentNullException(nameof(rocks));
			_sceneResourcesRepository =
				sceneResourcesRepository ?? throw new ArgumentNullException(nameof(sceneResourcesRepository));
		}

		private IResourceModel ResourceModelReadOnly => _persistentProgressService.GlobalProgress.ResourceModel;

		public IResourcesProgressPresenter Create() =>
			new ResourcesProgressPresenter(
				ResourceModelReadOnly,
				ResourceModelReadOnly,
				_fillMeshShaderProvider,
				new SandParticlePlayerSystem(
					_sandParticleView,
					_coroutineRunner,
					1
				),
				_playerModelRepositoryProvider.Get(ProgressType.MaxCashScore),
				_triggerSell,
				_coroutineRunner,
				_rocks,
				_sceneResourcesRepository
			);
	}
}