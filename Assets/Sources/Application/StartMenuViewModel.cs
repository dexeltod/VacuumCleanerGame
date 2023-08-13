using System.Collections.Generic;
using Sources.Application.MainMenu;
using Sources.Application.StateMachineInterfaces;
using Sources.DIService;
using Sources.Infrastructure.UI;
using Sources.InfrastructureInterfaces;
using Sources.Services;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

namespace Sources.Application
{
	[RequireComponent(typeof(UIElementGetterFacade))]
	public class StartMenuViewModel : MonoBehaviour
	{
		[SerializeField] private AudioMixer _audioMixer;
		[SerializeField] private AudioSource _buttonSound;
		
		private const string MainMenu = "MainMenu";
		private const string Settings = "Settings";

		private VisualElement _startMenu;
		private UIElementGetterFacade _uiElementGetter;

		private VisualElementViewModel _visualElementSwitcher;

		private List<Button> _allButtons = new();
		private ISceneConfigGetter _sceneConfigGetter;
		private IGameStateMachine _gameStateMachine;

		private void Start()
		{
			_gameStateMachine = GameServices.Container.Get<IGameStateMachine>();
			_sceneConfigGetter = GameServices.Container.Get<ISceneConfigGetter>();
			_uiElementGetter = GetComponent<UIElementGetterFacade>();
			_visualElementSwitcher = new VisualElementViewModel();

			CreateMenuWindows();
			_allButtons = _uiElementGetter.GetAllByType<Button>();
			SubscribeOnButtons();
		}

		private void OnDestroy() => 
			UnsubscribeFromButtons();

		private void CreateMenuWindows()
		{
			var mainMenu = new MainMenu.MainMenu(
				_uiElementGetter.GetFirst<VisualElement>(MenuVisualElementNames.Menu), 
				_uiElementGetter,
				_visualElementSwitcher,
				_sceneConfigGetter,
				_gameStateMachine
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