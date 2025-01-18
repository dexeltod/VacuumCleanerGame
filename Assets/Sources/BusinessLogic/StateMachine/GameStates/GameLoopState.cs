using System;
using Sources.BuisenessLogic.ServicesInterfaces;
using Sources.BuisenessLogic.States.StateMachineInterfaces;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.PresentationInterfaces;

namespace Sources.BuisenessLogic.StateMachine.GameStates
{
	public sealed class GameLoopState : IGameState
	{
		private readonly ILocalizationService _localizationService;
		private readonly IUpgradeWindowPresenter _upgradeWindowPresenter;
		private readonly IGameplayInterfacePresenter _gameplayInterfacePresenter;
		private readonly IResourcesProgressPresenter _resourcesProgressPresenter;
		private readonly IDissolveShaderViewController _dissolveShaderViewController;

		private readonly IGameMenuPresenter _gameMenuPresenter;
		private readonly IAdvertisementPresenter _advertisementHandler;
		private readonly IPersistentProgressService _persistentProgressService;
#if YANDEX_CODE
#endif

		private readonly ILoadingCurtain _loadingCurtain;

		public GameLoopState(
			ILoadingCurtain loadingCurtain,
			ILocalizationService localizationService,
			IUpgradeWindowPresenter upgradeWindowPresenter,
			IGameplayInterfacePresenter gameplayInterfacePresenter,
			IResourcesProgressPresenter resourcesProgressPresenter,
			IDissolveShaderViewController dissolveShaderViewController,
			IGameMenuPresenter gameMenuPresenter,
			IAdvertisementPresenter advertisementHandler,
			IPersistentProgressService persistentProgressService
		)
		{
			_localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
			_upgradeWindowPresenter = upgradeWindowPresenter ?? throw new ArgumentNullException(nameof(upgradeWindowPresenter));

			_gameplayInterfacePresenter =
				gameplayInterfacePresenter ?? throw new ArgumentNullException(nameof(gameplayInterfacePresenter));
			_resourcesProgressPresenter =
				resourcesProgressPresenter ?? throw new ArgumentNullException(nameof(resourcesProgressPresenter));
			_dissolveShaderViewController = dissolveShaderViewController ??
			                                throw new ArgumentNullException(nameof(dissolveShaderViewController));
			_gameMenuPresenter = gameMenuPresenter ?? throw new ArgumentNullException(nameof(gameMenuPresenter));
			_advertisementHandler = advertisementHandler ?? throw new ArgumentNullException(nameof(advertisementHandler));
			_persistentProgressService =
				persistentProgressService ?? throw new ArgumentNullException(nameof(persistentProgressService));

			_loadingCurtain = loadingCurtain ?? throw new ArgumentNullException(nameof(loadingCurtain));
		}

		private IDissolveShaderViewController DissolveShaderViewController =>
			_dissolveShaderViewController;

		private IResourcesProgressPresenter ResourcesProgressPresenter => _resourcesProgressPresenter;

		public void Enter()
		{
			SetMoreMoney();

			_localizationService.UpdateTranslations();

			_upgradeWindowPresenter.Enable();
			_gameplayInterfacePresenter.Enable();
			_resourcesProgressPresenter.Enable();
			_gameMenuPresenter.Enable();
			_advertisementHandler.Enable();

			_loadingCurtain.HideSlowly();
			DissolveShaderViewController.StartDissolving();
		}

		public void Exit()
		{
			_upgradeWindowPresenter.Disable();
			_gameplayInterfacePresenter.Disable();
			_resourcesProgressPresenter.Disable();
			_gameMenuPresenter.Disable();
			_loadingCurtain.Show();
			_advertisementHandler.Disable();
		}

		private void SetMoreMoney()
		{
#if DEV
			(_persistentProgressService.GlobalProgress.ResourceModelReadOnly as IResourceModel)!
				.AddMoney(
					999999
				);
#endif
		}
	}
}