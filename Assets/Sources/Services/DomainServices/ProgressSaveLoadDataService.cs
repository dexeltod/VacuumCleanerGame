using System;
using System.IO;
using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using Sources.Services.DomainServices.DTO;

namespace Sources.Services.DomainServices
{
	[Serializable] public class ProgressSaveLoadDataService : IProgressSaveLoadDataService
	{
		private readonly ISaveLoader _saveLoader;
		private readonly IPersistentProgressServiceProvider _progressServiceProvider;

		private readonly BinaryDataSaveLoader _binaryDataSaveLoader;
		private readonly JsonDataSaveLoader _jsonDataLoader;

		private IGameProgressProvider _gameProgress;

		public bool IsCallbackReceived { get; private set; }

		public event Func<IGameProgressProvider> ProgressCleared;

		private IPersistentProgressService PersistentProgressService => _progressServiceProvider.Implementation;

		public ProgressSaveLoadDataService(
			ISaveLoader saveLoader,
			IPersistentProgressServiceProvider progressService
		)
		{
			_saveLoader = saveLoader ?? throw new ArgumentNullException(nameof(saveLoader));
			_progressServiceProvider = progressService ?? throw new ArgumentNullException(nameof(progressService));
			_jsonDataLoader = new JsonDataSaveLoader();
		}

		public async UniTask SaveToCloud(IGameProgressProvider provider, Action succeededCallback = null)
		{
			if (provider != null)
				await Save(provider);

			succeededCallback?.Invoke();
		}

		public async UniTask SaveToCloud(Action succeededCallback = null)
		{
			await Save(PersistentProgressService.GameProgress);
			succeededCallback?.Invoke();
		}

		public async UniTask ClearSaves()
		{
			IGameProgressProvider clearedSave = ProgressCleared!.Invoke();
			_progressServiceProvider.Register<IPersistentProgressService>(new PersistentProgressService(clearedSave));

			await Save(clearedSave);
		}

		public async UniTask<IGameProgressProvider> LoadFromCloud()
		{
			IsCallbackReceived = false;
			return await _saveLoader.Load(() => IsCallbackReceived = true);
		}

		public void SaveToJson(string fileName, object data) =>
			_jsonDataLoader.Save(fileName, data);

		public string LoadFromJson(string fileName) =>
			_jsonDataLoader.Load(fileName);

		public T LoadFromJson<T>(string fileName) =>
			_jsonDataLoader.Load<T>(fileName);

		private async UniTask Save(IGameProgressProvider provider)
		{
			IsCallbackReceived = false;
			await _saveLoader.Save(provider, () => IsCallbackReceived = true);
		}
	}
}