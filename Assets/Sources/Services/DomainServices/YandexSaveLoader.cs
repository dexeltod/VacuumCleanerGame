#if YANDEX_CODE
using System;
using Cysharp.Threading.Tasks;
using Sources.ApplicationServicesInterfaces;
using Sources.Domain.Progress;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using UnityEngine;

namespace Sources.Services.DomainServices
{
	public class YandexSaveLoader : ISaveLoader
	{
		private readonly ICloudSave _yandexCloudSave;

		public YandexSaveLoader(ICloudSave yandexCloudSave) =>
			_yandexCloudSave = yandexCloudSave ?? throw new ArgumentNullException(nameof(yandexCloudSave));

		public async UniTask Save(IGlobalProgress @object)
		{
			if (@object == null) throw new ArgumentNullException(nameof(@object), "Save  is null");
			await _yandexCloudSave.Save(JsonUtility.ToJson(@object));
		}

		public async UniTask<IGlobalProgress> Load()
		{
			string json = await _yandexCloudSave.Load();

			Debug.Log("JSON" + json);

			if (string.IsNullOrEmpty(json) || json == "{}")
			{
				Debug.LogError("PROGRESS IS NULL");
				return null;
			}

			try
			{
				Debug.Log("Yandex saves loaded" + json);
				IGlobalProgress convertedJson = JsonUtility.FromJson<GlobalProgress>(json);
				return convertedJson;
			}
			catch (Exception e)
			{
				Debug.LogError(e + "RETURN NULL");
				throw new InvalidCastException("Json is wrong");
			}
		}

		public async UniTask ClearSaves(IGlobalProgress gameProgressModel) =>
			await _yandexCloudSave.DeleteSaves(gameProgressModel as GlobalProgress);

		public async UniTask Initialize() =>
			await _yandexCloudSave.Initialize();
	}
}
#endif