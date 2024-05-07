using System;
using Cysharp.Threading.Tasks;
using Sources.ApplicationServicesInterfaces;
using Sources.Domain.Progress;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using UnityEngine;

namespace Sources.Services.DomainServices
{
	public class UnitySaveLoader : ISaveLoader
	{
		private const string GameProgressKey = "GameProgress";

		private readonly IUnityServicesController _unityServicesController;
		private readonly ICloudSave _unityCloudSaveLoader;
		private IUnityServicesController _controller;

		public UnitySaveLoader(
			IUnityServicesController unityServicesController,
			ICloudSave unityCloudSaveLoader
		)
		{
			_unityServicesController = unityServicesController ??
				throw new ArgumentNullException(nameof(unityServicesController));
			_unityCloudSaveLoader
				= unityCloudSaveLoader ?? throw new ArgumentNullException(nameof(unityCloudSaveLoader));
		}

		public async UniTask Save(IGlobalProgress @object)
		{
			string json = JsonUtility.ToJson(@object);

			await _unityCloudSaveLoader.Save(json);
		}

		public async UniTask<IGlobalProgress> Load()
		{
			var json = await _unityCloudSaveLoader.Load();
			return DeserializeJson(json);
		}

		public async UniTask Initialize() =>
			await _unityServicesController.InitializeUnityServices();

		private IGlobalProgress DeserializeJson(string jsonSave)
		{
			try
			{
				GlobalProgress provider = JsonUtility.FromJson<GlobalProgress>(jsonSave);

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