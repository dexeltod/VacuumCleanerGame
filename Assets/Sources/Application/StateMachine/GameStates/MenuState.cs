using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sources.Application.StateMachineInterfaces;
using Sources.ApplicationServicesInterfaces;
using Sources.DIService;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs;
using Sources.View.SceneEntity;
using UnityEngine;

namespace Sources.Application.StateMachine.GameStates
{
	public class MenuState : IGameState
	{
#if !UNITY_EDITOR
		private const int LeaderBoardPlayersCount = 5;
#endif

		private readonly SceneLoader    _sceneLoader;
		private readonly LoadingCurtain _loadingCurtain;
		private readonly ServiceLocator   _serviceLocator;

		private LeaderBoardBehaviour _leaderBoardBehaviour;

		public MenuState(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, ServiceLocator serviceLocator)
		{
			_sceneLoader    = sceneLoader;
			_loadingCurtain = loadingCurtain;
			_serviceLocator   = serviceLocator;
		}

		public async void Enter()
		{
#if !UNITY_EDITOR
			ILeaderBoardService leaderBoardService = _gameServices.Get<ILeaderBoardService>();
#endif

			IAssetProvider assetProvider = _serviceLocator.Get<IAssetProvider>();

			await _sceneLoader.Load(ConstantNames.MenuScene);

			_leaderBoardBehaviour = InstantiateLeaderBoardBehaviour(assetProvider);

			_loadingCurtain.SetText("Loading leaders");

#if !UNITY_EDITOR
			InstantiateLeaders(await LoadLeaders(leaderBoardService));
#endif
			_loadingCurtain.HideSlowly();
		}

		private LeaderBoardBehaviour InstantiateLeaderBoardBehaviour(IAssetProvider assetProvider) =>
			assetProvider.InstantiateAndGetComponent<LeaderBoardBehaviour>
			(
				ResourcesAssetPath
					.Scene
					.UIResources
					.MainMenuCanvas
			);

		public void Exit()
		{
			_loadingCurtain.Show();
			_loadingCurtain.gameObject.SetActive(true);
		}

#if !UNITY_EDITOR
		private async UniTask<Dictionary<string, int>> LoadLeaders(ILeaderBoardService leaderBoardService) =>
			await leaderBoardService.GetLeaders(LeaderBoardPlayersCount);
#endif

		private void InstantiateLeaders(Dictionary<string, int> dictionary) =>
			_leaderBoardBehaviour.InstantiatePanels(dictionary);
	}
}