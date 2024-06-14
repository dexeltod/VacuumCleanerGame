using System;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.Services;
using Sources.Services.DomainServices;
using VContainer;

namespace Sources.Infrastructure.Services
{
	public class ProgressCleaner : IProgressCleaner
	{
		private readonly IInitialProgressFactory _initialProgressFactory;
		private readonly IPersistentProgressServiceProvider _progressServiceProvider;
		private readonly ProgressServiceRegister _progressServiceRegister;
		private readonly IProgressCleaner _progressCleaner;

		[Inject]
		public ProgressCleaner(
			IInitialProgressFactory initialProgressFactory,
			IPersistentProgressServiceProvider progressServiceProvider,
			ProgressServiceRegister progressServiceRegister
		)
		{
			_initialProgressFactory = initialProgressFactory ??
				throw new ArgumentNullException(nameof(initialProgressFactory));
			_progressServiceProvider = progressServiceProvider ??
				throw new ArgumentNullException(nameof(progressServiceProvider));
			_progressServiceRegister = progressServiceRegister ??
				throw new ArgumentNullException(nameof(progressServiceRegister));
		}

		public IGlobalProgress Clear()
		{
			IGlobalProgress clearedProgress = _initialProgressFactory.Create();

			_progressServiceProvider.Unregister();
			_progressServiceProvider.Register(new PersistentProgressService(clearedProgress));

			_progressServiceRegister.Do(clearedProgress);
			return clearedProgress;
		}
	}
}