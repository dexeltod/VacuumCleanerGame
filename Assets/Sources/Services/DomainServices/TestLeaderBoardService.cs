using System;
using System.Collections.Generic;
using System.Linq;
using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Services.DomainServices;

namespace Sources.Services.DomainServices
{
	public class LeadersPlug
	{
		private readonly Dictionary<string, int> _players;

		public LeadersPlug() =>
			_players = new Dictionary<string, int>()
			{
				{ "player1", 1 },
				{ "player2", 2 },
				{ "player3", 3 },
				{ "player4", 4 },
				{ "player5", 5 },
			};

		public Dictionary<string, int> GetTestLeaders() =>
			_players;
	}

	public class TestLeaderBoardService : IAbstractLeaderBoard
	{
		private readonly Dictionary<string, int> _leaders;

		public TestLeaderBoardService()
		{
			_leaders = new LeadersPlug().GetTestLeaders();
		}

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