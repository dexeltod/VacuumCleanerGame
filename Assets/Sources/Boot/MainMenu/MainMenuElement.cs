using System;
using Sources.BusinessLogic.Interfaces;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Infrastructure.Services;
using Sources.Infrastructure.UI;
using Sources.Utils.ConstantNames;
using UnityEngine;
using UnityEngine.UIElements;

namespace Sources.Boot.MainMenu
{
	public class MainMenuElement : MenuElement, IDisposable
	{
		private const string LevelsMenuElement = "Levels";
		private const string SettingsMenuElement = "Settings";
		private const string GameSceneName = "Game";
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly ILocalizationService _localizationService;

		private readonly UIElementGetterFacade _uiElementGetter;
		private readonly VisualElementSwitcher _visualElementSwitcher;

		private Button _exitButton;
		private Button _levelsButton;
		private VisualElement _levelsVisualElement;

		private VisualElement _menuVisualElement;

		private Button _playButton;
		private Button _settingsButton;
		private VisualElement _settingsVisualElement;

		public MainMenuElement(
			VisualElement thisElement,
			UIElementGetterFacade uiElementGetter,
			VisualElementSwitcher visualElementSwitcher,
			ILevelConfigGetter levelConfigGetter,
			ILocalizationService localizationService
		) : base(
			thisElement,
			visualElementSwitcher,
			uiElementGetter
		)
		{
			_uiElementGetter = uiElementGetter;
			_visualElementSwitcher = visualElementSwitcher;
			_levelConfigGetter = levelConfigGetter;
			_localizationService = localizationService;
			Initialize();
		}

		public void Dispose()
		{
			ReleaseUnmanagedResources();
			GC.SuppressFinalize(this);
		}

		private void GetButtons()
		{
			_playButton = _uiElementGetter.GetFirst<Button>(UiButtonNames.Play);
			string translation = _localizationService.GetTranslationText(UiButtonNames.Play);

			if (translation != null)
				_playButton.text = translation;
		}

		private void GetElementsToSwitch() => _levelsVisualElement = _uiElementGetter.GetFirst<VisualElement>(LevelsMenuElement);

		private void Initialize()
		{
			GetElementsToSwitch();
			GetButtons();
			SubscribeOnButtons();
		}

		private void OnExitGame() => Application.Quit();

		private void OnOpenLevelsMenu() => _visualElementSwitcher.Enter(ThisElement, _levelsVisualElement);

		private void OnPlay()
		{
			_visualElementSwitcher.Disable(ThisElement);
			// LevelConfig levelConfig = _levelConfigGetter.GetOrDefault(_)

			// _stateMachine.Enter<BuildSceneState, LevelConfig>(levelConfig);

			throw new NotImplementedException();
		}

		private void OpenSettingsMenu() => _visualElementSwitcher.Enter(ThisElement, _settingsVisualElement);

		private void ReleaseUnmanagedResources() => UnsubscribeFromButtons();

		private void SubscribeOnButtons() => _playButton.clicked += OnPlay;

		private void UnsubscribeFromButtons() => _playButton.clicked -= OnPlay;

		~MainMenuElement() => ReleaseUnmanagedResources();
	}
}