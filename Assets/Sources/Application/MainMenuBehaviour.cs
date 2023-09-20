using System;
using Sources.Application.StateMachine.GameStates;
using Sources.Application.StateMachineInterfaces;
using Sources.DIService;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Scene;
using Sources.Utils.Configs;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Application
{
	public class MainMenuBehaviour : MonoBehaviour, IDisposable
	{
		[SerializeField] private GameObject _settingsMenu;
		[SerializeField] private GameObject _mainMenu;

		[SerializeField] private Button _playButton;
		[SerializeField] private Button _settingsButton;
		[SerializeField] private Button _deletaSavesButton;
		[SerializeField] private Button _exitButton;

		private IGameStateMachine _gameStateMachine;
		private ISceneConfigGetter _sceneConfigGetter;
		private ISaveLoadDataService _saveLoadDataService;

		private void OnEnable()
		{
			_playButton.onClick.AddListener(OnPlay);
			_deletaSavesButton.onClick.AddListener(OnDeleteSaves);
			// _settingsButton.onClick.AddListener(OnSettings);
			// _exitButton.onClick.AddListener(OnExit);
		}

		private void OnDisable()
		{
			_playButton.onClick.RemoveListener(OnPlay);
			// _settingsButton.onClick.RemoveListener(OnSettings);
			// _exitButton.onClick.RemoveListener(OnExit);
		}

		private void Start()
		{
			_saveLoadDataService =  GameServices.Container.Get<ISaveLoadDataService>();
			_gameStateMachine = GameServices.Container.Get<IGameStateMachine>();
			_sceneConfigGetter = GameServices.Container.Get<ISceneConfigGetter>();
		}

		public void Dispose()
		{
			_playButton.onClick.RemoveListener(OnPlay);
			// _settingsButton.onClick.RemoveListener(OnSettings);
			// _exitButton.onClick.RemoveListener(OnExit);
		}

		public void OnPlay()
		{
			SceneConfig sceneConfig = _sceneConfigGetter.GetSceneConfig(ResourcesAssetPath.Configs.Game);
			_gameStateMachine.Enter<SceneLoadState, string>(sceneConfig.SceneName);
		}

		private async void OnDeleteSaves() => 
			await _saveLoadDataService.ClearSaves();

		public void OnSettings()
		{
			_mainMenu.gameObject.SetActive(false);
			_settingsMenu.gameObject.SetActive(true);
		}

		public void OnExit()
		{
			UnityEngine.Application.Quit();
		}
	}
}