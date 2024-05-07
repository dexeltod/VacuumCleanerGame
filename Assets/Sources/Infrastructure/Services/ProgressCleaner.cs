using System;
using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories.Player;
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
		private readonly IProgressCleaner _progressCleaner;

		[Inject]
		public ProgressCleaner(
			IInitialProgressFactory initialProgressFactory,
			IPersistentProgressServiceProvider progressServiceProvider
		)
		{
			_initialProgressFactory = initialProgressFactory ??
				throw new ArgumentNullException(nameof(initialProgressFactory));
			_progressServiceProvider = progressServiceProvider ??
				throw new ArgumentNullException(nameof(progressServiceProvider));
		}

		public IGlobalProgress CreateClearSaves()
		{
			IGlobalProgress clearedProgress = _initialProgressFactory.Create();
			_progressServiceProvider.Register(new PersistentProgressService(clearedProgress));

			return clearedProgress;
		}
	}
}