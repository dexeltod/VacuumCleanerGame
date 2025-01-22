using System;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.BusinessLogic.States.StateMachineInterfaces;
using Sources.ControllersInterfaces;
using Sources.ControllersInterfaces.Services;
using Sources.DomainInterfaces;
using Sources.PresentationInterfaces;
using VContainer;

namespace Sources.Boot.Scripts.States.StateMachine.GameStates
{
	public sealed class GameLoopState : IGameState
	{
		private readonly ILocalizationService _localizationService;
		private readonly IResourcesProgressPresenter _resourcesProgressPresenter;
		private readonly IDissolveShaderViewController _dissolveShaderViewController;

		private readonly IPersistentProgressService _persistentProgressService;
		private readonly IPresentersContainerRepository _presentersContainerRepository;
#if YANDEX_CODE
#endif

		private readonly ILoadingCurtain _loadingCurtain;

		[Inject]
		public GameLoopState(
			ILoadingCurtain loadingCurtain,
			ILocalizationService localizationService,
			IPersistentProgressService persistentProgressService,
			IPresentersContainerRepository presentersContainerRepository
		)
		{
			_localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));

			_persistentProgressService =
				persistentProgressService ?? throw new ArgumentNullException(nameof(persistentProgressService));
			_presentersContainerRepository = presentersContainerRepository ??
			                                 throw new ArgumentNullException(nameof(presentersContainerRepository));

			_loadingCurtain = loadingCurtain ?? throw new ArgumentNullException(nameof(loadingCurtain));
		}

		public void Enter()
		{
			SetMoreMoney();

			_localizationService.UpdateTranslations();

			_presentersContainerRepository.EnableAll();

			_loadingCurtain.HideSlowly();
			_dissolveShaderViewController.StartDissolving();
		}

		public void Exit()
		{
			_presentersContainerRepository.DisableAll();
		}

		private void SetMoreMoney()
		{
#if DEV

			_persistentProgressService.GlobalProgress.ResourceModelReadOnly!.AddMoney(999999);
#endif
		}
	}
}