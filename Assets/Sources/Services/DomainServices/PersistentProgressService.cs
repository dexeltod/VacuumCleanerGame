using System;
using Sources.DomainInterfaces;
using Sources.ServicesInterfaces;

namespace Sources.Services.DomainServices
{
	[Serializable]
	public class PersistentProgressService : IPersistentProgressService
	{
		private IGameProgressModel _gameProgress;
		public IGameProgressModel GameProgress => _gameProgress;

		public void Construct(IGameProgressModel gameProgress)
		{
			if (GameProgress != null)
				return;

			_gameProgress = gameProgress;
		}
	}
}