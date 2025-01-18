using System;
using Sources.BusinessLogic.Interfaces.Factory;
using Sources.BusinessLogic.Services;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories.Progress;
using VContainer;

namespace Sources.Infrastructure.Services
{
	public class ClearProgressFactory : IClearProgressFactory
	{
		private readonly IInitialProgressFactory _initialProgressFactory;
		private readonly IPersistentProgressServiceUpdatable _progressServiceProvider;
		private readonly ProgressServiceRegister _progressServiceRegister;

		[Inject]
		public ClearProgressFactory(
			IInitialProgressFactory initialProgressFactory,
			IPersistentProgressServiceUpdatable progressServiceProvider,
			ProgressServiceRegister progressServiceRegister
		)
		{
			_initialProgressFactory = initialProgressFactory ?? throw new ArgumentNullException(nameof(initialProgressFactory));
			_progressServiceProvider = progressServiceProvider ?? throw new ArgumentNullException(nameof(progressServiceProvider));
		}

		public IGlobalProgress Create()
		{
			IGlobalProgress clearedProgress = _initialProgressFactory.Create();

			_progressServiceProvider.Update(clearedProgress);

			_progressServiceRegister.Do(clearedProgress);
			return clearedProgress;
		}
	}
}
