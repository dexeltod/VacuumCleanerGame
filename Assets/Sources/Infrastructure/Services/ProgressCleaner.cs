using System;
using Sources.BusinessLogic.Interfaces.Factory;
using Sources.BusinessLogic.Services;
using Sources.DomainInterfaces;
using VContainer;

namespace Sources.Infrastructure.Services
{
	public class ProgressCleaner : IProgressCleaner
	{
		private readonly IInitialProgressFactory _initialProgressFactory;
		private readonly IUpdatablePersistentProgressService _progressService;

		[Inject]
		public ProgressCleaner(
			IInitialProgressFactory initialProgressFactory,
			IUpdatablePersistentProgressService progressServiceProvider
		)
		{
			_initialProgressFactory = initialProgressFactory ?? throw new ArgumentNullException(nameof(initialProgressFactory));
			_progressService = progressServiceProvider ?? throw new ArgumentNullException(nameof(progressServiceProvider));
		}

		public IGlobalProgress CreateClear()
		{
			IGlobalProgress clearedProgress = _initialProgressFactory.Create();

			_progressService.Update(clearedProgress);

			return clearedProgress;
		}
	}
}
