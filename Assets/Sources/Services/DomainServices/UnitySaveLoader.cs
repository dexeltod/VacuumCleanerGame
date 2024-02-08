using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sources.ApplicationServicesInterfaces;
using Sources.Domain.Progress;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Unity.Services.CloudSave;
using UnityEngine;

namespace Sources.Services.DomainServices
{
	public class UnitySaveLoader : ISaveLoader
	{
		private const string GameProgressKey = "GameProgress";

		private readonly IPersistentProgressService _progressService;
		private readonly IUnityServicesController _unityServicesController;
		private readonly ICloudSave _unityCloudSaveLoader;
		private IUnityServicesController _controller;

		public UnitySaveLoader(
			IPersistentProgressService progressService,
			IUnityServicesController unityServicesController,
			ICloudSave unityCloudSaveLoader
		)
		{
			_progressService = progressService ?? throw new ArgumentNullException(nameof(progressService));
			_unityServicesController = unityServicesController ??
				throw new ArgumentNullException(nameof(unityServicesController));
			_unityCloudSaveLoader
				= unityCloudSaveLoader ?? throw new ArgumentNullException(nameof(unityCloudSaveLoader));
		}

		public async UniTask Save(IGameProgressProvider @object, Action succeededCallback)
		{
			GameProgressProvider provider = _progressService.GameProgress as GameProgressProvider;
			string json = JsonUtility.ToJson(provider);

			await _unityCloudSaveLoader.Save(json);
		}

		public async UniTask<IGameProgressProvider> Load(Action callback)
		{
			var json = await _unityCloudSaveLoader.Load();
			callback.Invoke();
			return DeserializeJson(json);
		}

		public async UniTask Initialize() =>
			await _unityServicesController.InitializeUnityServices();

		public async UniTask ClearSaves(IGameProgressProvider gameProgressProvider, Action succeededCallback)
		{
			await CloudSaveService.Instance.Data.ForceDeleteAsync(GameProgressKey);
			succeededCallback.Invoke();
		}

		private IGameProgressProvider DeserializeJson(string jsonSave)
		{
			try
			{
				GameProgressProvider provider = JsonUtility.FromJson<GameProgressProvider>(jsonSave);

				return provider;
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				Debug.Log("New progress will be created");
				return null;
			}
		}
	}
}