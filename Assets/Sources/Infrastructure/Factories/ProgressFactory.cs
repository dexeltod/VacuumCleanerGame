using System;
using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces;
using Sources.Infrastructure.DataViewModel;
using Sources.Infrastructure.Factories.Domain;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces.Providers;
using Sources.Services.DomainServices;
using Sources.ServicesInterfaces;
using Sources.Utils.ConstantNames;
using UnityEngine;

namespace Sources.Infrastructure.Factories.Player
{
	[Serializable] public class ProgressFactory : IProgressFactory
	{
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly IPersistentProgressServiceProvider _persistentProgressServiceProvider;
		private readonly IPlayerStatsServiceProvider _playerStatsServiceProvider;
		private readonly InitialProgressFactory _initialProgressFactory;
		private readonly ProgressConstantNames _progressConstantNames;
		private readonly IProgressCleaner _progressCleaner;

		private IPlayerProgressSetterFacadeProvider _playerProgressSetterFacadeProvider;

		public ProgressFactory(
			IProgressSaveLoadDataService progressSaveLoadDataService,
			InitialProgressFactory initialProgressFactory,
			ProgressConstantNames progressConstantNames,
			IPersistentProgressServiceProvider persistentProgressServiceProvider,
			IPlayerStatsServiceProvider playerStatsService,
			IPlayerProgressSetterFacadeProvider playerProgressSetterFacadeProvider,
			ISaveLoaderProvider saveLoaderProvider,
			IProgressCleaner progressCleaner
		)
		{
			_progressSaveLoadDataService = progressSaveLoadDataService ??
				throw new ArgumentNullException(nameof(progressSaveLoadDataService));
			_initialProgressFactory = initialProgressFactory ??
				throw new ArgumentNullException(nameof(initialProgressFactory));
			_progressConstantNames
				= progressConstantNames ?? throw new ArgumentNullException(nameof(progressConstantNames));
			_persistentProgressServiceProvider = persistentProgressServiceProvider ??
				throw new ArgumentNullException(nameof(persistentProgressServiceProvider));
			_playerStatsServiceProvider
				= playerStatsService ?? throw new ArgumentNullException(nameof(playerStatsService));
			_playerProgressSetterFacadeProvider = playerProgressSetterFacadeProvider ??
				throw new ArgumentNullException(nameof(playerProgressSetterFacadeProvider));

			_progressCleaner = progressCleaner ?? throw new ArgumentNullException(nameof(progressCleaner));
		}

		private IPersistentProgressService PersistentProgressService =>
			_persistentProgressServiceProvider.Implementation;

		public async UniTask<IGlobalProgress> Load() =>
			await _progressSaveLoadDataService.LoadFromCloud();

		public async UniTask Save(IGlobalProgress provider)
		{
			if (provider == null) throw new ArgumentNullException(nameof(provider));
			await _progressSaveLoadDataService.SaveToCloud();
		}

		public async UniTask Initialize()
		{
			var cloudSaves = await _progressSaveLoadDataService.LoadFromCloud();

			cloudSaves = await CreatNewIfNull(cloudSaves);

			RegisterServices(cloudSaves);
		}

		private async UniTask<IGlobalProgress> CreatNewIfNull(IGlobalProgress loadedProgress)
		{
			if (loadedProgress != null)
				return loadedProgress;

			Debug.Log("New progress model created");

			loadedProgress = await _progressCleaner.ClearAndSaveCloud();

			RegisterServices(loadedProgress);

			return loadedProgress;
		}

		private void RegisterServices(IGlobalProgress cloudSaves)
		{
			RegisterProgressServiceProvider(new PersistentProgressService(cloudSaves));

			_playerProgressSetterFacadeProvider.Register<IProgressSetterFacade>(
				new ProgressSetterFacade(
					_playerStatsServiceProvider,
					_progressSaveLoadDataService,
					_persistentProgressServiceProvider
				)
			);
		}

		private void RegisterProgressServiceProvider(IPersistentProgressService globalProgress) =>
			_persistentProgressServiceProvider.Register<IPersistentProgressService>(globalProgress);
	}
}