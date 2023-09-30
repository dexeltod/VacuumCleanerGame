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
		private readonly GameServices   _gameServices;

		private LeaderBoardBehaviour _leaderBoardBehaviour;

		public MenuState(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, GameServices gameServices)
		{
			_sceneLoader    = sceneLoader;
			_loadingCurtain = loadingCurtain;
			_gameServices   = gameServices;
		}

		public async void Enter()
		{
#if !UNITY_EDITOR
			ILeaderBoardService leaderBoardService = _gameServices.Get<ILeaderBoardService>();
#endif

			IAssetProvider assetProvider = _gameServices.Get<IAssetProvider>();

			await _sceneLoader.Load(ConstantNames.MenuScene);

			_leaderBoardBehaviour =
				assetProvider.InstantiateAndGetComponent<LeaderBoardBehaviour>
				(
					ResourcesAssetPath
						.Scene
						.UI
						.MainMenuCanvas
				);

			_loadingCurtain.SetText("Loading leaders");

#if !UNITY_EDITOR
			InstantiateLeaders(await LoadLeaders(leaderBoardService));
#endif
			_loadingCurtain.HideSlow();
		}

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