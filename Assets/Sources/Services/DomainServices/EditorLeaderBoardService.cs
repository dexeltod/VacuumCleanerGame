using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.Services.DomainServices
{
	public class EditorLeaderBoardService : ILeaderBoard
	{
		public UniTask Set(int score) =>
			UniTask.CompletedTask;

		public UniTask<Dictionary<string, int>> GetPlayers(int playersCount)
		{
			throw new System.NotImplementedException();
		}
	}
}