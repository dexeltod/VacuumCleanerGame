using Cysharp.Threading.Tasks;
using Sources.ApplicationServicesInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.Services
{
	public class LeaderBoardService : ILeaderBoardService
	{
		private readonly ILeaderBoardInfo _leaderboard;

		public LeaderBoardService(ILeaderBoardInfo leaderboard)
		{
			_leaderboard = leaderboard;
		}

		public async UniTask Set(int score)
		{
		}

		public void GetPlayer()
		{
		}

		public void GetLeaders()
		{
		}

		public async UniTask AddScore(int score)
		{
			await _leaderboard.SetScore(_leaderboard.Score + score);
		}
	}
}