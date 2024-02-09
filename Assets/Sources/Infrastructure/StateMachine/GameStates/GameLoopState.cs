using System;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces.States.StateMachineInterfaces;
using Sources.Presentation.SceneEntity;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using VContainer;

namespace Sources.Infrastructure.StateMachine.GameStates
{
	public sealed class GameLoopState : IGameState
	{
		private readonly GameplayInterfaceProvider _gameplayInterfaceProvider;
		private readonly ILocalizationService _localizationService;
		private readonly UpgradeWindowPresenterProvider _upgradeWindowPresenterProvider;
		private readonly MeshPresenterProvider _meshPresenterProvider;
		private readonly LoadingCurtain _loadingCurtain;

		private IGameplayInterfaceView GameplayInterface => _gameplayInterfaceProvider.Implementation;

		[Inject]
		public GameLoopState(
			LoadingCurtain loadingCurtain,
			GameplayInterfaceProvider gameplayInterfaceProvider,
			ILocalizationService localizationService,
			UpgradeWindowPresenterProvider upgradeWindowPresenterProvider,
			MeshPresenterProvider meshPresenterProvider
 
		)
		{
			_gameplayInterfaceProvider = gameplayInterfaceProvider ??
				throw new ArgumentNullException(nameof(gameplayInterfaceProvider));
			_localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
			_upgradeWindowPresenterProvider
				= upgradeWindowPresenterProvider ??
				throw new ArgumentNullException(nameof(upgradeWindowPresenterProvider));
			_meshPresenterProvider
				= meshPresenterProvider ?? throw new ArgumentNullException(nameof(meshPresenterProvider));
			_loadingCurtain = loadingCurtain ? loadingCurtain : throw new ArgumentNullException(nameof(loadingCurtain));
		}

		public void Enter()
		{
			_localizationService.UpdateTranslations();

			if (GameplayInterface == null)
				throw new NullReferenceException("GameplayInterface");

			GameplayInterface.GameObject.SetActive(true);

			_upgradeWindowPresenterProvider.Implementation.Enable();
			_meshPresenterProvider.Implementation.Enable();
			_loadingCurtain.HideSlowly();
		}

		public void Exit()
		{
			GameplayInterface?.GameObject.SetActive(false);

			_upgradeWindowPresenterProvider.Implementation.Disable();
			_meshPresenterProvider.Implementation.Disable();
			_loadingCurtain.Show();
		}
	}
}