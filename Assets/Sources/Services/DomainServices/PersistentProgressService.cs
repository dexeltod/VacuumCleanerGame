using System;
using Sources.DomainInterfaces;

namespace Sources.Services.DomainServices
{
	[Serializable] public class PersistentProgressService : IPersistentProgressService
	{
		private IGameProgressModel _gameProgress;
		public IGameProgressModel GameProgress => _gameProgress;

		public void Set(IGameProgressModel gameProgress) =>
			_gameProgress = gameProgress ?? throw new ArgumentNullException(nameof(gameProgress));
	}
}