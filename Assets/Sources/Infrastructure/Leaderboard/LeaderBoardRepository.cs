using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sources.BusinessLogic.Interfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.Infrastructure.Leaderboard
{
	public class LeaderBoardRepository : ILeaderBoardService
	{
		private readonly IAbstractLeaderBoard _leaderboard;

		public LeaderBoardRepository(IAbstractLeaderBoard leaderboard) =>
			_leaderboard = leaderboard ?? throw new ArgumentNullException(nameof(leaderboard));

		public async UniTask<Dictionary<string, int>> GetLeaders(int playerCount) => await _leaderboard.GetPlayers(playerCount);

		public async UniTask AddScore(int score) => await _leaderboard.AddScore(score);

		public async UniTask<Tuple<string, int>> GetLeader()
		{
			Dictionary<string, int> player = await _leaderboard.GetPlayers(1);

			return new Tuple<string, int>(player.Keys.First(), player.Values.First());
		}
	}
}