using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.Services.DomainServices
{
	public class TestLeaderBoardService : IAbstractLeaderBoard
	{
		public UniTask AddScore(int newScore)
		{
			throw new NotImplementedException();
		}

		public UniTask Set(int score) =>
			UniTask.CompletedTask;

		public UniTask<Dictionary<string, int>> GetPlayers(int playersCount)
		{
			throw new NotImplementedException();
		}

		public UniTask<Tuple<string, int>> GetPlayer()
		{
			throw new NotImplementedException();
		}
	}
}