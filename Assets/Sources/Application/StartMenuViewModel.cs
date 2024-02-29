using System.Collections.Generic;
using Sources.Application.MainMenu;
using Sources.Infrastructure.UI;
using Sources.Services;
using Sources.ServicesInterfaces;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;
using VContainer.Unity;

namespace Sources.Application
{

	[RequireComponent(typeof(UIElementGetterFacade))]
	public class StartMenuViewModel : MonoBehaviour, IPostStartable
	{
		[SerializeField] private AudioMixer _audioMixer;
		[SerializeField] private AudioSource _buttonSound;

		private const string MainMenu = "MainMenu";
		private const string Settings = "Settings";

		private VisualElement _startMenu;
		private UIElementGetterFacade _uiElementGetter;

		private VisualElementSwitcher _visualElementSwitcher;

		private List<Button> _allButtons = new();
		private ILevelConfigGetter _levelConfigGetter;
		private ILocalizationService _localization;


		private void Construct(
			ILevelConfigGetter levelConfigGetter,
			ILocalizationService localization
		)
		{
			_visualElementSwitcher = new VisualElementSwitcher();
		}

		public void PostStart()
		{
			_uiElementGetter = GetComponent<UIElementGetterFacade>();

			CreateMenuWindows();
			_allButtons = _uiElementGetter.GetAllByType<Button>();
			SubscribeOnButtons();
		}

		private void OnDestroy() =>
			UnsubscribeFromButtons();

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
			foreach (var button in _allButtons)
				button.clicked += PlayButtonSound;
		}

		private void UnsubscribeFromButtons()
		{
			foreach (var button in _allButtons)
				button.clicked -= PlayButtonSound;
		}

		private void PlayButtonSound() =>
			_buttonSound.Play();
	}
}