using Sources.Core.Domain.Progress;
using Sources.DomainServices.Interfaces;

namespace Sources.DomainServices
{
	public class PersistentProgressService : IPersistentProgressService
	{
		public GameProgressModel GameProgress { get; private set; }

		public void Construct(GameProgressModel gameProgress)
		{
			if (GameProgress != null)
				return;

			GameProgress = gameProgress;
		}
	}
}