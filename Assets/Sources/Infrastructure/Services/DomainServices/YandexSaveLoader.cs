#if YANDEX_CODE
using System;
using Sources.Domain.Progress;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using UnityEngine;

namespace Sources.Infrastructure.Services.DomainServices
{
	public class YandexSaveLoader : ISaveLoader
	{
		private readonly ICloudSave _yandexController;

		public YandexSaveLoader(ICloudSave yandexCloudSave) =>
			_yandexController = yandexCloudSave ?? throw new ArgumentNullException(nameof(yandexCloudSave));

		public async UniTask Save(IGlobalProgress @object, Action succeededCallback = null)
		{
			await _yandexController.Save(JsonUtility.ToJson((GlobalProgress)@object));

			succeededCallback?.Invoke();
		}

		public async UniTask<IGlobalProgress> Load(Action succeededCallback)
		{
			string json = await _yandexController.Load();

			Debug.Log("JSON" + json);

			if (string.IsNullOrEmpty(json) || json == "{}")
			{
				Debug.LogError("PROGRESS IS NULL");
				return null;
			}

			try
			{
				Debug.Log("" + json);
				IGlobalProgress convertedJson = JsonUtility.FromJson<GlobalProgress>(json);
				succeededCallback.Invoke();
				return convertedJson;
			}
			catch (Exception e)
			{
				Debug.LogError(e + "RETURN NULL");
				throw new InvalidCastException("Json is wrong");
			}
		}

		public async UniTask ClearSaves(IGlobalProgress gameProgressModel, Action succeededCallback)
		{
			await _yandexController.DeleteSaves(gameProgressModel);
			succeededCallback.Invoke();
		}

		public async UniTask Initialize()
		{
			await YandexGamesSdk.Initialize();
			YandexGamesSdk.GameReady();
		}
	}
}
#endif