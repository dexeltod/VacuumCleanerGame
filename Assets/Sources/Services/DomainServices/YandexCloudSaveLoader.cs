using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Sources.ApplicationServicesInterfaces;
using Sources.DomainInterfaces;
using UnityEngine;

namespace Sources.Services.DomainServices
{
	public sealed class YandexCloudSaveLoader : ICloudSave
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

		public async UniTask Initialize()
		{
			await YandexGamesSdk.Initialize();
			YandexGamesSdk.GameReady();
		}
	}
}