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

		public async UniTask Save(IGameProgressModel @object, Action succeededCallback)
		{
			GameProgressModel model = _progressService.GameProgress as GameProgressModel;
			string json = JsonUtility.ToJson(model);

			await _unityCloudSaveLoader.Save(json);
		}

		public async UniTask<IGameProgressModel> Load(Action callback)
		{
			var json = await _unityCloudSaveLoader.Load();
			callback.Invoke();
			return DeserializeJson(json);
		}

		public async UniTask Initialize() =>
			await _unityServicesController.InitializeUnityServices();

		public async UniTask ClearSaves(IGameProgressModel gameProgressModel, Action succeededCallback)
		{
			await CloudSaveService.Instance.Data.ForceDeleteAsync(GameProgressKey);
			succeededCallback.Invoke();
		}

		private IGameProgressModel DeserializeJson(string jsonSave)
		{
			try
			{
				GameProgressModel model = JsonUtility.FromJson<GameProgressModel>(jsonSave);

				return model;
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