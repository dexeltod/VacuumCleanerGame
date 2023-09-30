using System;
using Cysharp.Threading.Tasks;
using Sources.Application.StateMachine.GameStates;
using Sources.Application.StateMachineInterfaces;
using Sources.ApplicationServicesInterfaces;
using Sources.DIService;
using Sources.DomainInterfaces;
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
		[SerializeField] private Button _deleteSavesButton;
		[SerializeField] private Button _exitButton;
		[SerializeField] private Button _addScoreButton;

		private IGameStateMachine    _gameStateMachine;
		private ISceneConfigGetter   _sceneConfigGetter;
		private IProgressLoadDataService _progressLoadDataService;
		private ILeaderBoardService  _leaderBoardService;

		private void OnEnable()
		{
			_addScoreButton.onClick.AddListener(OnAddLeader);
			_playButton.onClick.AddListener(OnPlay);
			_deleteSavesButton.onClick.AddListener(OnDeleteSaves);
			// _settingsButton.onClick.AddListener(OnSettings);
			// _exitButton.onClick.AddListener(OnExit);
		}

		private void OnDisable()
		{
			_playButton.onClick.RemoveListener(OnPlay);
			_addScoreButton.onClick.RemoveListener(OnAddLeader);
			_deleteSavesButton.onClick.RemoveListener(OnDeleteSaves);
			// _settingsButton.onClick.RemoveListener(OnSettings);
			// _exitButton.onClick.RemoveListener(OnExit);
		}

		private void Start()
		{
			_leaderBoardService  = GameServices.Container.Get<ILeaderBoardService>();
			_progressLoadDataService = GameServices.Container.Get<IProgressLoadDataService>();
			_gameStateMachine    = GameServices.Container.Get<IGameStateMachine>();
			_sceneConfigGetter   = GameServices.Container.Get<ISceneConfigGetter>();
		}

		public void Dispose()
		{
			_playButton.onClick.RemoveListener(OnPlay);
			// _settingsButton.onClick.RemoveListener(OnSettings);
			// _exitButton.onClick.RemoveListener(OnExit);
		}

		private void OnPlay()
		{
			SceneConfig sceneConfig = _sceneConfigGetter.GetSceneConfig(ResourcesAssetPath.Configs.Game);
			_gameStateMachine.Enter<SceneLoadState, string>(sceneConfig.SceneName);
		}

		private async void OnAddLeader() => await _leaderBoardService.AddScore(200);

		private async void OnDeleteSaves() => await _progressLoadDataService.ClearSaves();

		private void OnSettings()
		{
			_mainMenu.gameObject.SetActive(false);
			_settingsMenu.gameObject.SetActive(true);
		}

		private void OnExit() => UnityEngine.Application.Quit();
	}
}