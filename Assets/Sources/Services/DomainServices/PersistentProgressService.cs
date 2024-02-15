using System;
using Sources.DomainInterfaces;

namespace Sources.Services.DomainServices
{
	[Serializable] public class PersistentProgressService : IPersistentProgressService
	{
		private IGameProgressProvider _gameProgress;
		public IGameProgressProvider GameProgress => _gameProgress;

		public PersistentProgressService(IGameProgressProvider gameProgress) =>
			_gameProgress = gameProgress ?? throw new ArgumentNullException(nameof(gameProgress));
	}
}