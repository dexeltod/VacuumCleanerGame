using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Sources.BusinessLogic.Interfaces
{
	public interface ILeaderBoardService
	{
		UniTask AddScore(int score);
		UniTask<Dictionary<string, int>> GetLeaders(int playerCount);
	}
}