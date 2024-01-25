using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sources.Application;
using Sources.ApplicationServicesInterfaces;
using Sources.Presentation.UI.MainMenu.LeaderBoard;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;
using UnityEngine;

namespace Sources.Infrastructure.Factories.LeaderBoard
{
	public class LeaderBoardFactory
	{
		private const int LeaderBoardPlayersCount = 5;

		private readonly IAssetProvider _assetProvider;
		private readonly ILeaderBoardService _leaderBoardService;

		private LeaderBoardBehaviour _leaderBoardBehaviour;

		public MainMenuBehaviour MainMenuBehaviour { get; private set; }

		public LeaderBoardFactory(IAssetProvider assetProvider, ILeaderBoardService leaderBoardService)
		{
			_assetProvider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));
			_leaderBoardService = leaderBoardService ?? throw new ArgumentNullException(nameof(leaderBoardService));
		}

		public async UniTask<LeaderBoardBehaviour> Instantiate()
		{
			GameObject gameObject = _assetProvider.Instantiate
			(
				ResourcesAssetPath
					.Scene
					.UIResources
					.MainMenuCanvas
			);

			_leaderBoardBehaviour = gameObject.GetComponent<LeaderBoardBehaviour>();
			MainMenuBehaviour = gameObject.GetComponent<MainMenuBehaviour>();

			InstantiateLeaders(await _leaderBoardService.GetLeaders(LeaderBoardPlayersCount));

			return _leaderBoardBehaviour;
		}

		private void InstantiateLeaders(Dictionary<string, int> leaders) =>
			_leaderBoardBehaviour.InstantiatePanels(leaders);
	}
}