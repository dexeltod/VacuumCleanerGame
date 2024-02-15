using System;
using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.States.StateMachineInterfaces;
using Sources.Presentation.Player;
using Sources.Presentation.SceneEntity;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using Sources.Utils.ConstantNames;
using VContainer;

namespace Sources.Infrastructure.StateMachine.GameStates
{
	public sealed class GameLoopState : IGameState
	{
		private readonly GameplayInterfaceProvider _gameplayInterfaceProvider;
		private readonly ILocalizationService _localizationService;
		private readonly UpgradeWindowPresenterProvider _upgradeWindowPresenterProvider;
		private readonly IGameplayInterfacePresenterProvider _gameplayInterfacePresenterProvider;
		private readonly IResourcesProgressPresenterProvider _resourcesProgressPresenterProvider;
		private readonly DissolveShaderViewControllerProvider _dissolveShaderViewControllerProvider;
		private readonly LoadingCurtain _loadingCurtain;

		private IGameplayInterfaceView GameplayInterface => _gameplayInterfaceProvider.Implementation;

		private IDissolveShaderViewController DissolveShaderViewController =>
			_dissolveShaderViewControllerProvider.Implementation;

		private IResourcesProgressPresenter ResourcesProgressPresenter =>
			_resourcesProgressPresenterProvider.Implementation;

		[Inject]
		public GameLoopState(
			LoadingCurtain loadingCurtain,
			GameplayInterfaceProvider gameplayInterfaceProvider,
			ILocalizationService localizationService,
			UpgradeWindowPresenterProvider upgradeWindowPresenterProvider,
			IGameplayInterfacePresenterProvider gameplayInterfacePresenterProvider,
			IResourcesProgressPresenterProvider resourcesProgressPresenterProvider,
			DissolveShaderViewControllerProvider dissolveShaderViewControllerProvider
		)
		{
			_gameplayInterfaceProvider = gameplayInterfaceProvider ??
				throw new ArgumentNullException(nameof(gameplayInterfaceProvider));
			_localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
			_upgradeWindowPresenterProvider
				= upgradeWindowPresenterProvider ??
				throw new ArgumentNullException(nameof(upgradeWindowPresenterProvider));

			_gameplayInterfacePresenterProvider = gameplayInterfacePresenterProvider ??
				throw new ArgumentNullException(nameof(gameplayInterfacePresenterProvider));
			_resourcesProgressPresenterProvider = resourcesProgressPresenterProvider ??
				throw new ArgumentNullException(nameof(resourcesProgressPresenterProvider));
			_dissolveShaderViewControllerProvider = dissolveShaderViewControllerProvider ??
				throw new ArgumentNullException(nameof(dissolveShaderViewControllerProvider));
			_loadingCurtain = loadingCurtain ? loadingCurtain : throw new ArgumentNullException(nameof(loadingCurtain));
		}

		public void Enter()
		{
			_localizationService.UpdateTranslations();

			if (GameplayInterface == null)
				throw new NullReferenceException("GameplayInterface");

			GameplayInterface.GameObject.SetActive(true);

			_upgradeWindowPresenterProvider.Implementation.Enable();
			_gameplayInterfacePresenterProvider.Implementation.Enable();
			_resourcesProgressPresenterProvider.Implementation.Enable();
			ResourcesProgressPresenter.Enable();
			_loadingCurtain.HideSlowly();

			DissolveShaderViewController.StartDissolving();
		}

		public void Exit()
		{
			GameplayInterface?.GameObject.SetActive(false);

			_upgradeWindowPresenterProvider.Implementation.Disable();
			_gameplayInterfacePresenterProvider.Implementation.Disable();
			_resourcesProgressPresenterProvider.Implementation.Disable();
			ResourcesProgressPresenter.Disable();
			_loadingCurtain.Show();
		}
	}
}