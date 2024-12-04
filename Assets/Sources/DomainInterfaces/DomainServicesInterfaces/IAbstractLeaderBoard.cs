using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface IAbstractLeaderBoard
	{
		UniTask AddScore(int newScore);
		UniTask Set(int score);

		UniTask<Dictionary<string, int>> GetPlayers(int playersCount);
		UniTask<Tuple<string, int>> GetPlayer();
	}
}