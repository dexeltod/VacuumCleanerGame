using System;
using Cysharp.Threading.Tasks;
using Sources.InfrastructureInterfaces.Factory;

namespace Sources.Infrastructure.Factories
{
    public class DebugLeaderFactory
    {
        private readonly ILeaderBoardPlayersFactory _leaderBoardPlayersFactory;

        public DebugLeaderFactory(ILeaderBoardPlayersFactory leaderBoardPlayersFactory) =>
            _leaderBoardPlayersFactory = leaderBoardPlayersFactory ??
                                         throw new ArgumentNullException(nameof(leaderBoardPlayersFactory));

        public UniTask Create() =>
            _leaderBoardPlayersFactory.Create();
    }
}