using System;
using Sources.ApplicationServicesInterfaces.StateMachineInterfaces;
using Sources.Infrastructure.Presenters;
using Sources.Presentation.SceneEntity;
using Sources.PresentationInterfaces;
using Sources.Services.Interfaces;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.UI;
using VContainer;

namespace Sources.Application.StateMachine.GameStates
{
	public sealed class GameLoopState : IGameState
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly IUIGetter _uiGetter;
		private readonly IUpgradeWindowPresenter _upgradeWindowPresenter;
		private readonly ILevelChangerPresenter _levelChangerPresenter;
		private readonly ILocalizationService _localizationService;
		private readonly LoadingCurtain _loadingCurtain;
		private IGameplayInterfaceView _gameplayInterface;

		[Inject]
		public GameLoopState(
			GameStateMachine gameStateMachine,
			LoadingCurtain loadingCurtain,
			IUIGetter uiGetter,
			IUpgradeWindowPresenter upgradeWindowPresenter,
			ILevelChangerPresenter levelChangerPresenter,
			ILocalizationService localizationService
		)
		{
			_gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
			_uiGetter = uiGetter ?? throw new ArgumentNullException(nameof(uiGetter));
			_upgradeWindowPresenter = upgradeWindowPresenter ??
				throw new ArgumentNullException(nameof(upgradeWindowPresenter));
			_levelChangerPresenter = levelChangerPresenter ?? throw new ArgumentNullException(nameof(levelChangerPresenter));
			_localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
			_loadingCurtain = loadingCurtain ? loadingCurtain : throw new ArgumentNullException(nameof(loadingCurtain));
		}

		public void Enter()
		{
			_localizationService.UpdateTranslations();
			
			if (_uiGetter.GameplayInterface != null)
				_gameplayInterface = _uiGetter.GameplayInterface;
			else
				throw new ArgumentNullException(nameof(_uiGetter.GameplayInterface));

			_gameplayInterface.GameObject.SetActive(true);

			_upgradeWindowPresenter.Enable();
			_levelChangerPresenter.Enable();

			_loadingCurtain.HideSlowly();
		}

		public void Exit()
		{
			if (_gameplayInterface != null)
			{
				_gameplayInterface.Destroying -= OnDestroying;
				_gameplayInterface.GameObject.SetActive(false);
			}

			_upgradeWindowPresenter.Disable();
			_levelChangerPresenter.Disable();
			
			_loadingCurtain.Show();
		}

		private void OnDestroying() =>
			_gameplayInterface = null;
	}
}