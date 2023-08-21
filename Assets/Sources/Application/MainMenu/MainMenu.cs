using System;
using Sources.Application.StateMachine.GameStates;
using Sources.Application.StateMachineInterfaces;
using Sources.Application.Utils.Configs;
using Sources.Infrastructure.UI;
using Sources.InfrastructureInterfaces;
using Sources.Services;
using Sources.ServicesInterfaces;
using UnityEngine.UIElements;

namespace Sources.Application.MainMenu
{
	public class MainMenu : MenuElement, IDisposable
	{
		private const string LevelsMenuElement = "Levels";
		private const string SettingsMenuElement = "Settings";
		private const string GameSceneName = "Game";

		private readonly UIElementGetterFacade _uiElementGetter;
		private readonly VisualElementSwitcher _visualElementSwitcher;
		private readonly ISceneConfigGetter _sceneConfigGetter;
		private readonly IGameStateMachine _gameStateMachine;
		private readonly ILocalizationService _localizationService;
		private VisualElement _menuVisualElement;
		private VisualElement _levelsVisualElement;
		private VisualElement _settingsVisualElement;

		private Button _playButton;
		private Button _levelsButton;
		private Button _settingsButton;

		private Button _exitButton;

		public MainMenu
		(
			VisualElement thisElement,
			UIElementGetterFacade uiElementGetter,
			VisualElementSwitcher visualElementSwitcher,
			ISceneConfigGetter sceneConfigGetter,
			IGameStateMachine gameStateMachine,
			ILocalizationService localizationService
		) : base
		(
			thisElement,
			visualElementSwitcher,
			uiElementGetter
		)
		{
			_uiElementGetter = uiElementGetter;
			_visualElementSwitcher = visualElementSwitcher;
			_sceneConfigGetter = sceneConfigGetter;
			_gameStateMachine = gameStateMachine;
			_localizationService = localizationService;
			Initialize();
		}

		private void Initialize()
		{
			GetElementsToSwitch();
			GetButtons();
			SubscribeOnButtons();
		}

		public void Dispose()
		{
			ReleaseUnmanagedResources();
			GC.SuppressFinalize(this);
		}

		~MainMenu()
		{
			ReleaseUnmanagedResources();
		}

		private void GetElementsToSwitch()
		{
			_levelsVisualElement = _uiElementGetter.GetFirst<VisualElement>(LevelsMenuElement);
		}

		private void GetButtons()
		{
			_playButton = _uiElementGetter.GetFirst<Button>(UiButtonNames.Play);
			string translation = _localizationService.GetTranslationText(UiButtonNames.Play);

			if (translation != null)
				_playButton.text = translation;
		}

		private void SubscribeOnButtons()
		{
			_playButton.clicked += OnPlay;
		}

		private void UnsubscribeFromButtons()
		{
			_playButton.clicked -= OnPlay;
		}

		private void OnExitGame() =>
			UnityEngine.Application.Quit();

		private void OnOpenLevelsMenu() =>
			_visualElementSwitcher.Enter(ThisElement, _levelsVisualElement);

		private void OpenSettingsMenu() =>
			_visualElementSwitcher.Enter(ThisElement, _settingsVisualElement);

		private void OnPlay()
		{
			_visualElementSwitcher.Disable(ThisElement);
			SceneConfig sceneConfig = _sceneConfigGetter.GetSceneConfig(ResourcesAssetPath.GameObjects.Game);

			_gameStateMachine.Enter<SceneLoadState, string>(sceneConfig.SceneName);
		}

		private void ReleaseUnmanagedResources()
		{
			UnsubscribeFromButtons();
		}
	}
}