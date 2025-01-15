using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Sources.BuisenessLogic.Interfaces
{
	public interface ILeaderBoardService
	{
		UniTask<Dictionary<string, int>> GetLeaders(int playerCount);
		UniTask AddScore(int score);
	}
}