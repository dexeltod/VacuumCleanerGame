using System;
using ViewModel.Infrastructure.Services;

namespace Model.Infrastructure.Data
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