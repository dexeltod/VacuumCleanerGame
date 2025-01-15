using System;
using Sources.BuisenessLogic.Interfaces.Factory;
using Sources.BuisenessLogic.Services;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories.Progress;
using Sources.Infrastructure.Services.DomainServices;
using VContainer;

namespace Sources.Infrastructure.Services
{
	public class ProgressCleaner : IProgressCleaner
	{
		private readonly IInitialProgressFactory _initialProgressFactory;
		private readonly IPersistentProgressService _progressServiceProvider;
		private readonly ProgressServiceRegister _progressServiceRegister;
		private readonly PersistentProgressService _persistentProgressService;
		private readonly IProgressCleaner _progressCleaner;

		[Inject]
		public ProgressCleaner(
			IInitialProgressFactory initialProgressFactory,
			IPersistentProgressService progressServiceProvider,
			ProgressServiceRegister progressServiceRegister
		)
		{
			_initialProgressFactory = initialProgressFactory ?? throw new ArgumentNullException(nameof(initialProgressFactory));
			_progressServiceProvider = progressServiceProvider ?? throw new ArgumentNullException(nameof(progressServiceProvider));
		}

		public IGlobalProgress Clear()
		{
			IGlobalProgress clearedProgress = _initialProgressFactory.Create();

			_persistentProgressService.Update(clearedProgress);

			_progressServiceRegister.Do(clearedProgress);
			return clearedProgress;
		}
	}
}