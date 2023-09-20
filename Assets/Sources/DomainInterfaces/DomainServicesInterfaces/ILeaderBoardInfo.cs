using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface ILeaderBoardInfo
	{
		int Score { get; }
		UniTask<Dictionary<string, int>> GetPlayers(int playerCount);
		UniTask SetScore(int score);
	}
}