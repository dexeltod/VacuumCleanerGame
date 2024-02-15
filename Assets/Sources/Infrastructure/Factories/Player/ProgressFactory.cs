using System;
using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Factories.Domain;
using Sources.InfrastructureInterfaces.Providers;
using Sources.Services.DomainServices;
using Sources.Utils.ConstantNames;
using UnityEngine;

namespace Sources.Infrastructure.Factories.Player
{
	[Serializable] public class ProgressFactory : IDisposable
	{
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly IPersistentProgressServiceProvider _persistentProgressServiceProvider;
		private readonly InitialProgressFactory _initialProgressFactory;
		private readonly ProgressConstantNames _progressConstantNames;

		public ProgressFactory(
			IProgressSaveLoadDataService progressSaveLoadDataService,
			InitialProgressFactory initialProgressFactory,
			ProgressConstantNames progressConstantNames,
			IPersistentProgressServiceProvider persistentProgressServiceProvider
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

			_progressSaveLoadDataService.ProgressCleared += _initialProgressFactory.Create;
		}

		public void Dispose() =>
			_progressSaveLoadDataService.ProgressCleared -= _initialProgressFactory.Create;

		public async UniTask Initialize()
		{
			CreatNewIfNull(await _progressSaveLoadDataService.LoadFromCloud());
			
			IGameProgressProvider cloudSaves = await _progressSaveLoadDataService.LoadFromCloud();
			IPersistentProgressService persistentProgressService = new PersistentProgressService(cloudSaves);
			_persistentProgressServiceProvider.Register<IPersistentProgressService>(persistentProgressService);
		}

		public async UniTask<IGameProgressProvider> Load() =>
			await _progressSaveLoadDataService.LoadFromCloud();

		public void Save(IGameProgressProvider provider) =>
			_progressSaveLoadDataService.SaveToCloud(provider);

		public void LoadClearProgress() =>
			Save(_initialProgressFactory.Create());

		private IGameProgressProvider CreatNewIfNull(IGameProgressProvider loadedProgress)
		{
			if (loadedProgress != null)
				return loadedProgress;

			Debug.Log("New progress model created");

			loadedProgress = _initialProgressFactory.Create();
			_progressSaveLoadDataService.SaveToCloud(loadedProgress);

			return loadedProgress;
		}
	}
}