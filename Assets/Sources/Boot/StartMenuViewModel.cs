using System.Collections.Generic;
using Sources.Boot.MainMenu;
using Sources.BusinessLogic.Interfaces;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Infrastructure.Services;
using Sources.Infrastructure.UI;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;
using VContainer.Unity;

namespace Sources.Boot
{
	[RequireComponent(typeof(UIElementGetterFacade))]
	public class StartMenuViewModel : MonoBehaviour, IPostStartable
	{
		private const string MainMenu = "MainMenu";
		private const string Settings = "Settings";
		[SerializeField] private AudioMixer _audioMixer;
		[SerializeField] private AudioSource _buttonSound;

		private List<Button> _allButtons = new();
		private ILevelConfigGetter _levelConfigGetter;
		private ILocalizationService _localization;

		private VisualElement _startMenu;
		private UIElementGetterFacade _uiElementGetter;

		private VisualElementSwitcher _visualElementSwitcher;

		private void OnDestroy() =>
			UnsubscribeFromButtons();

		public void PostStart()
		{
			_uiElementGetter = GetComponent<UIElementGetterFacade>();

			CreateMenuWindows();
			_allButtons = _uiElementGetter.GetAllByType<Button>();
			SubscribeOnButtons();
		}

		private void Construct(
			ILevelConfigGetter levelConfigGetter,
			ILocalizationService localization
		)
		{
			_visualElementSwitcher = new VisualElementSwitcher();
		}

		private void CreateMenuWindows()
		{
			var mainMenu = new MainMenuElement(
				_uiElementGetter.GetFirst<VisualElement>(MenuVisualElementNames.Menu),
				_uiElementGetter,
				_visualElementSwitcher,
				_levelConfigGetter,
				_localization
			);
		}

		private void SubscribeOnButtons()
		{
			foreach (Button button in _allButtons)
				button.clicked += PlayButtonSound;
		}

		private void UnsubscribeFromButtons()
		{
			foreach (Button button in _allButtons)
				button.clicked -= PlayButtonSound;
		}

		private void PlayButtonSound() =>
			_buttonSound.Play();
	}
}