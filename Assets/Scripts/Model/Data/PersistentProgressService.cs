using ViewModel.Infrastructure.Services;

namespace Model.Data
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