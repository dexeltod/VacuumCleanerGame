using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sources.DIService;

namespace Sources.ApplicationServicesInterfaces
{
	public interface ILeaderBoardService : IService
	{
		UniTask<Dictionary<string, int>> GetLeaders(int playerCount);
		UniTask                          AddScore(int   score);
	}
}