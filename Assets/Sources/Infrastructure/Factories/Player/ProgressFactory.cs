using System;
using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Utils.ConstantNames;
using UnityEngine;

namespace Sources.Infrastructure.Factories.Player
{
	[Serializable] public class ProgressFactory : IDisposable
	{
#region Fields

		private readonly IProgressLoadDataService _progressLoadDataService;
		private readonly IPersistentProgressServiceConstructable _persistentProgressService;
		private readonly InitialProgressFactory _initialProgressFactory;

#endregion

		public ProgressFactory(
			IProgressLoadDataService progressLoadDataService,
			IPersistentProgressServiceConstructable persistentProgressService,
			InitialProgressFactory initialProgressFactory,
			ProgressConstantNames progressConstantNames
		)
		{
			_progressLoadDataService = progressLoadDataService ??
				throw new ArgumentNullException(nameof(progressLoadDataService));
			_persistentProgressService = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));
			_initialProgressFactory = initialProgressFactory ??
				throw new ArgumentNullException(nameof(initialProgressFactory));

			_progressLoadDataService.ProgressCleared += _initialProgressFactory.Create;
		}

		public void Dispose() =>
			_progressLoadDataService.ProgressCleared -= _initialProgressFactory.Create;

		public async UniTask InitializeProgress()
		{
			IGameProgressModel loadedSaves = await _progressLoadDataService.LoadFromCloud();
			Initialize(loadedSaves);
		}

		public async UniTask<IGameProgressModel> Load() =>
			await _progressLoadDataService.LoadFromCloud();

		public void Save(IGameProgressModel model) =>
			_progressLoadDataService.SaveToCloud(model);

		private void Initialize(IGameProgressModel loadedProgress)
		{
			loadedProgress = CreatNewIfNull(loadedProgress);
			_persistentProgressService.Set(loadedProgress);
		}

		private IGameProgressModel CreatNewIfNull(IGameProgressModel loadedProgress)
		{
			if (loadedProgress == null)
			{
				Debug.Log("new progress model");

				loadedProgress = _initialProgressFactory.Create();
				_progressLoadDataService.SaveToCloud(loadedProgress);
			}

			return loadedProgress;
		}
	}
}