using System;
using Cysharp.Threading.Tasks;
using Sources.BusinessLogic.Interfaces;
using Sources.Domain.Progress;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using UnityEngine;

namespace Sources.Infrastructure.Services.DomainServices
{
	public class UnitySaveLoader : ISaveLoader
	{
		private const string GameProgressKey = "GameProgress";
		private readonly ICloudSaveLoader _unityCloudSaveLoaderLoader;

		private readonly UnityServicesOptions _unityServicesOptions;
		private UnityServicesOptions _controller;

		public UnitySaveLoader(
			UnityServicesOptions unityServicesOptions,
			ICloudSaveLoader unityCloudSaveLoaderLoader
		)
		{
			_unityServicesOptions = unityServicesOptions ?? throw new ArgumentNullException(nameof(unityServicesOptions));
			_unityCloudSaveLoaderLoader
				= unityCloudSaveLoaderLoader ?? throw new ArgumentNullException(nameof(unityCloudSaveLoaderLoader));
		}

		public async UniTask Save(IGlobalProgress @object, Action succeededCallback)
		{
			string json = JsonUtility.ToJson(@object);

			await _unityCloudSaveLoaderLoader.Save(json);
		}

		public async UniTask<IGlobalProgress> Load() => DeserializeJson(await _unityCloudSaveLoaderLoader.Load());

		public async UniTask Initialize() => await _unityServicesOptions.InitializeUnityServices();

		private IGlobalProgress DeserializeJson(string jsonSave)
		{
			try
			{
				var provider = JsonUtility.FromJson<GlobalProgress>(jsonSave);

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
