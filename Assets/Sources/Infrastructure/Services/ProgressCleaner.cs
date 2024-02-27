using System;
using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories.Player;
using Sources.Infrastructure.Providers;
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
		private readonly ISaveLoaderProvider _saveLoader;
		private readonly IPersistentProgressServiceProvider _progressServiceProvider;
		private readonly PlayerStatsFactory _playerStatsFactory;
		private readonly IProgressCleaner _progressCleaner;

		[Inject]
		public ProgressCleaner(
			IInitialProgressFactory initialProgressFactory,
			ISaveLoaderProvider saveLoader,
			IPersistentProgressServiceProvider progressServiceProvider,
			PlayerStatsFactory playerStatsFactory
		)
		{
			_initialProgressFactory = initialProgressFactory ??
				throw new ArgumentNullException(nameof(initialProgressFactory));
			_saveLoader = saveLoader;
			_progressServiceProvider = progressServiceProvider ??
				throw new ArgumentNullException(nameof(progressServiceProvider));
			_playerStatsFactory = playerStatsFactory ?? throw new ArgumentNullException(nameof(playerStatsFactory));
		}

		public async UniTask<IGlobalProgress> ClearAndSaveCloud()
		{
			IGlobalProgress clearedProgress = _initialProgressFactory.Create();
			_progressServiceProvider.Register(new PersistentProgressService(clearedProgress));
			_playerStatsFactory.Create();
			await _saveLoader.Implementation.Save(clearedProgress);

			return clearedProgress;
		}
	}
}