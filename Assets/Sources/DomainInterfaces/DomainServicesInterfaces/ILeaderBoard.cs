using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface ILeaderBoard
	{
		UniTask Set(int score);

		UniTask<Dictionary<string, int>> GetPlayers(int playersCount);
	}
}