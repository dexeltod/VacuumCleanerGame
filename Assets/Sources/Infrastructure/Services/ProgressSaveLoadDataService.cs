using System;
using Cysharp.Threading.Tasks;
using Sources.BusinessLogic.Interfaces.Factory;
using Sources.BusinessLogic.Services;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Services.DomainServices;
using UnityEngine;
using VContainer;

namespace Sources.Infrastructure.Services
{
	[Serializable]
	public class ProgressSaveLoadDataService : IProgressSaveLoadDataService
	{
		private readonly BinaryDataSaveLoader _binaryDataSaveLoader;
		private readonly IClearProgressFactory _clearProgressFactory;
		private readonly JsonDataSaveLoader _jsonDataLoader;
		private readonly IPersistentProgressService _progressService;

		private IGlobalProgress _globalProgress;

		[Inject]
		public ProgressSaveLoadDataService(
			ISaveLoader saveLoader,
			IPersistentProgressService progressService,
			IInitialProgressFactory initialProgressFactory,
			IClearProgressFactory clearProgressFactory
		)
		{
			SaveLoaderImplementation = saveLoader ?? throw new ArgumentNullException(nameof(saveLoader));
			_progressService = progressService ?? throw new ArgumentNullException(nameof(progressService));
			_clearProgressFactory = clearProgressFactory ?? throw new ArgumentNullException(nameof(clearProgressFactory));

			_jsonDataLoader = new JsonDataSaveLoader();
		}

		private ISaveLoader SaveLoaderImplementation { get; }

		public bool IsCallbackReceived { get; private set; }

		public async UniTask SaveToCloud(Action succeededCallback = null)
		{
			await Save(_progressService.GlobalProgress);
			succeededCallback?.Invoke();
		}

		public async UniTask ClearSaves() =>
			await SaveLoaderImplementation.Save(_clearProgressFactory.Create());

		public async UniTask<IGlobalProgress> LoadFromCloud()
		{
			Debug.Log("LoadFromCloud");
			IsCallbackReceived = false;
			return await SaveLoaderImplementation.Load(() => IsCallbackReceived = true);
		}

		public void SaveToJson(string fileName, object data) =>
			_jsonDataLoader.Save(fileName, data);

		public string LoadFromJson(string fileName) =>
			_jsonDataLoader.Load(fileName);

		public T LoadFromJson<T>(string fileName) =>
			_jsonDataLoader.Load<T>(fileName);

		private async UniTask Save(IGlobalProgress provider)
		{
			IsCallbackReceived = false;
			await SaveLoaderImplementation.Save(provider, () => IsCallbackReceived = true);
		}
	}
}