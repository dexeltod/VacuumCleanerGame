using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface IAbstractLeaderBoard
	{
		UniTask AddScore(int newScore);
		UniTask<Tuple<string, int>> GetPlayer();

		UniTask<Dictionary<string, int>> GetPlayers(int playersCount);
		UniTask Set(int score);
	}
}