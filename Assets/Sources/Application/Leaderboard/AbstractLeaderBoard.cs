using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sources.ApplicationServicesInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.Application.Leaderboard
{
	public class AbstractLeaderBoard : ILeaderBoardService
	{
		private readonly IAbstractLeaderBoard _leaderboard;

		public AbstractLeaderBoard(IAbstractLeaderBoard leaderboard) => _leaderboard = leaderboard;

		public async UniTask<Tuple<string, int>> GetLeader()
		{
			//TODO: need implementation
			return null;
		}

		public async UniTask<Dictionary<string, int>> GetLeaders(int playerCount) =>
			await _leaderboard.GetPlayers(playerCount);

		public async UniTask AddScore(int score) => await _leaderboard.AddScore(score);
	}
}