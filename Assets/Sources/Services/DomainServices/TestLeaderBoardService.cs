using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.Services.DomainServices
{
	public class TestLeaderBoardService : IAbstractLeaderBoard
	{
		private readonly Dictionary<string, int> _leaders;

		public TestLeaderBoardService() =>
			_leaders = new LeadersPlug().GetTestLeaders();

		public UniTask AddScore(int newScore)
		{
			throw new NotImplementedException();
		}

		public UniTask Set(int score) =>
			UniTask.CompletedTask;

		public async UniTask<Dictionary<string, int>> GetPlayers(int playersCount) =>
			_leaders;

		public async UniTask<Tuple<string, int>> GetPlayer() =>
			new Tuple<string, int>("player1", 1);
	}
}