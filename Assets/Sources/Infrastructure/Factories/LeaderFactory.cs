using System;
using Cysharp.Threading.Tasks;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Factory;

namespace Sources.Infrastructure.Factories
{
	public class LeaderFactory : ILeaderFactory
	{
		private readonly ILeaderBoardPlayersFactory _leaderBoardPlayersFactory;

		public LeaderFactory(ILeaderBoardPlayersFactory leaderBoardPlayersFactory) =>
			_leaderBoardPlayersFactory = leaderBoardPlayersFactory ??
				throw new ArgumentNullException(nameof(leaderBoardPlayersFactory));

		public UniTask Create() =>
			_leaderBoardPlayersFactory.Create();
	}
}