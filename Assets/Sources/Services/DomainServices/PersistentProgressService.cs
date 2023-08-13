using Sources.Domain.Progress;
using Sources.DomainInterfaces;
using Sources.ServicesInterfaces;

namespace Sources.Services.DomainServices
{
	public class PersistentProgressService : IPersistentProgressService
	{
		public IGameProgressModel GameProgress { get; private set; }

		public void Construct(IGameProgressModel gameProgress)
		{
			if (GameProgress != null)
				return;

			GameProgress = gameProgress;
		}
	}
}