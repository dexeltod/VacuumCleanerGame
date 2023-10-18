using System;
using Sources.Application.StateMachine.GameStates;
using Sources.Application.StateMachineInterfaces;
using Sources.Infrastructure.UI;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Scene;
using Sources.Services;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs;
using UnityEngine.UIElements;

namespace Sources.Application.MainMenu
{
	public class MainMenuElement : MenuElement, IDisposable
	{
		private const string LevelsMenuElement   = "Levels";
		private const string SettingsMenuElement = "Settings";
		private const string GameSceneName       = "Game";

		private readonly UIElementGetterFacade _uiElementGetter;
		private readonly VisualElementSwitcher _visualElementSwitcher;
		private readonly ILevelConfigGetter    _levelConfigGetter;
		private readonly IGameStateMachine     _gameStateMachine;
		private readonly ILocalizationService  _localizationService;

		private VisualElement _menuVisualElement;
		private VisualElement _levelsVisualElement;
		private VisualElement _settingsVisualElement;

		private Button _playButton;
		private Button _levelsButton;
		private Button _settingsButton;

		private Button _exitButton;

		public MainMenuElement
		(
			VisualElement         thisElement,
			UIElementGetterFacade uiElementGetter,
			VisualElementSwitcher visualElementSwitcher,
			ILevelConfigGetter    levelConfigGetter,
			IGameStateMachine     gameStateMachine,
			ILocalizationService  localizationService
		) : base
		(
			thisElement,
			visualElementSwitcher,
			uiElementGetter
		)
		{
			_uiElementGetter       = uiElementGetter;
			_visualElementSwitcher = visualElementSwitcher;
			_levelConfigGetter     = levelConfigGetter;
			_gameStateMachine      = gameStateMachine;
			_localizationService   = localizationService;
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

		~MainMenuElement() =>
			ReleaseUnmanagedResources();

		private void GetElementsToSwitch() =>
			_levelsVisualElement = _uiElementGetter.GetFirst<VisualElement>(LevelsMenuElement);

		private void GetButtons()
		{
			_playButton = _uiElementGetter.GetFirst<Button>(UiButtonNames.Play);
			string translation = _localizationService.GetTranslationText(UiButtonNames.Play);

			if (translation != null)
				_playButton.text = translation;
		}

		private void SubscribeOnButtons() =>
			_playButton.clicked += OnPlay;

		private void UnsubscribeFromButtons() =>
			_playButton.clicked -= OnPlay;

		private void OnExitGame() =>
			UnityEngine.Application.Quit();

		private void OnOpenLevelsMenu() =>
			_visualElementSwitcher.Enter(ThisElement, _levelsVisualElement);

		private void OpenSettingsMenu() =>
			_visualElementSwitcher.Enter(ThisElement, _settingsVisualElement);

		private void OnPlay()
		{
			_visualElementSwitcher.Disable(ThisElement);
			LevelConfig levelConfig = _levelConfigGetter.GetCurrentLevel();

			_gameStateMachine.Enter<BuildSceneState, LevelConfig>(levelConfig);
		}

		private void ReleaseUnmanagedResources() =>
			UnsubscribeFromButtons();
	}
}