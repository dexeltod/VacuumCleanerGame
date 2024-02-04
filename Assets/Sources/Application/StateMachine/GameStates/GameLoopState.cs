using System;
using Sources.ApplicationServicesInterfaces.StateMachineInterfaces;
using Sources.Infrastructure.Providers;
using Sources.Presentation.SceneEntity;
using Sources.PresentationInterfaces;
using Sources.PresentersInterfaces;
using Sources.ServicesInterfaces;
using VContainer;

namespace Sources.Application.StateMachine.GameStates
{
	public sealed class GameLoopState : IGameState
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly GameplayInterfaceProvider _gameplayInterfaceProvider;
		private readonly IUpgradeWindowPresenter _upgradeWindowPresenter;
		private readonly ILevelChangerPresenter _levelChangerPresenter;
		private readonly ILocalizationService _localizationService;
		private readonly LoadingCurtain _loadingCurtain;

		private IGameplayInterfaceView GameplayInterface => _gameplayInterfaceProvider.Instance;

		[Inject]
		public GameLoopState(
			GameStateMachine gameStateMachine,
			LoadingCurtain loadingCurtain,
			GameplayInterfaceProvider gameplayInterfaceProvider,
			IUpgradeWindowPresenter upgradeWindowPresenter,
			ILevelChangerPresenter levelChangerPresenter,
			ILocalizationService localizationService
		)
		{
			_gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
			_gameplayInterfaceProvider = gameplayInterfaceProvider ??
				throw new ArgumentNullException(nameof(gameplayInterfaceProvider));
			_upgradeWindowPresenter = upgradeWindowPresenter ??
				throw new ArgumentNullException(nameof(upgradeWindowPresenter));
			_levelChangerPresenter
				= levelChangerPresenter ?? throw new ArgumentNullException(nameof(levelChangerPresenter));
			_localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
			_loadingCurtain = loadingCurtain ? loadingCurtain : throw new ArgumentNullException(nameof(loadingCurtain));
		}

		public void Enter()
		{
			_localizationService.UpdateTranslations();

			if (GameplayInterface == null)
				throw new NullReferenceException("GameplayInterface");

			GameplayInterface.GameObject.SetActive(true);

			_upgradeWindowPresenter.Enable();
			_levelChangerPresenter.Enable();

			_loadingCurtain.HideSlowly();
		}

		public void Exit()
		{
			GameplayInterface?.GameObject.SetActive(false);

			_upgradeWindowPresenter.Disable();
			_levelChangerPresenter.Disable();

			_loadingCurtain.Show();
		}
	}
}