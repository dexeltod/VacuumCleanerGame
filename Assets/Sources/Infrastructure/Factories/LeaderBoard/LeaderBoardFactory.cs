using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sources.ApplicationServicesInterfaces;
using Sources.Presentation.UI.MainMenu.LeaderBoard;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs;
using Sources.Utils.Configs.Scripts;

namespace Sources.Infrastructure.Factories.LeaderBoard
{
	public class LeaderBoardFactory
	{
		private const int LeaderBoardPlayersCount = 5;

		private readonly IAssetProvider _assetProvider;
		private readonly ILeaderBoardService _leaderBoardService;

		private LeaderBoardBehaviour _leaderBoardBehaviour;

		public LeaderBoardFactory(IAssetProvider assetProvider, ILeaderBoardService leaderBoardService)
		{
			_assetProvider = assetProvider;
			_leaderBoardService = leaderBoardService;
		}

		public async UniTask<LeaderBoardBehaviour> Create()
		{
			_leaderBoardBehaviour = _assetProvider.InstantiateAndGetComponent<LeaderBoardBehaviour>
			(
				ResourcesAssetPath
					.Scene
					.UIResources
					.MainMenuCanvas
			);

			InstantiateLeaders(await _leaderBoardService.GetLeaders(LeaderBoardPlayersCount));

			return _leaderBoardBehaviour;
		}

		private void InstantiateLeaders(Dictionary<string, int> leaders) =>
			_leaderBoardBehaviour.InstantiatePanels(leaders);
	}
}