using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Sources.ApplicationServicesInterfaces;
using Sources.DomainInterfaces;
using UnityEngine;

namespace Sources.Services.DomainServices
{
	public class YandexCloudSaveLoader : ICloudSave
	{
		public async UniTask Save(string json)
		{
			bool isCallbackReceived = false;

			PlayerAccount.SetCloudSaveData(json, () => isCallbackReceived = true);
			await UniTask.WaitWhile(() => isCallbackReceived == false);
			Debug.Log("Player data saved" + json);
		}

		public async UniTask<string> Load()
		{
			bool isCallbackReceived = false;
			string json = "";

			PlayerAccount.GetCloudSaveData(
				successCallback =>
				{
					isCallbackReceived = true;
					json = successCallback;
				},
				errorCallback =>
				{
					Debug.LogError(errorCallback);

					isCallbackReceived = true;
					json = null;
				}
			);

			await UniTask.WaitWhile(() => isCallbackReceived == false);

			return json;
		}

		public async UniTask DeleteSaves(IGlobalProgress globalProgress)
		{
			bool isCallbackReceived = false;
			PlayerAccount.SetCloudSaveData("{}", () => isCallbackReceived = true);

			await UniTask.WaitWhile(() => isCallbackReceived == false);

			Debug.Log("DATA DELETED");
		}
	}

#if YANDEX_CODE
	public class YandexSaveLoader : ISaveLoader
	{
		private readonly ICloudSave _yandexController;

		public YandexSaveLoader(ICloudSave yandexCloudSave) =>
			_yandexController = yandexCloudSave ?? throw new ArgumentNullException(nameof(yandexCloudSave));

		public async UniTask Save(IGlobalProgress @object, Action succeededCallback = null)
		{
			await _yandexController.Save(JsonUtility.ToJson(@object));

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
				IGlobalProgress convertedJson = JsonUtility.FromJson<IGlobalProgress>(json);
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

		public UniTask Initialize()
		{
			return UniTask.CompletedTask;
		}
	}
#endif
}