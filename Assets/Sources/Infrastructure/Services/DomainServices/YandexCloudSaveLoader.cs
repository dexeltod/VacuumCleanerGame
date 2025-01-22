using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Sources.BusinessLogic.Interfaces;
using Sources.DomainInterfaces;
using UnityEngine;

namespace Sources.Infrastructure.Services.DomainServices
{
	public class YandexCloudSaveLoader : ICloudSaveLoader
	{
		public async UniTask Save(string json)
		{
			var isCallbackReceived = false;

			PlayerAccount.SetCloudSaveData(json, () => isCallbackReceived = true);

			await UniTask.WaitWhile(() => isCallbackReceived == false);
			Debug.Log("Player data saved" + json);
		}

		public async UniTask<string> Load()
		{
			var isCallbackReceived = false;
			var json = "";

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
			var isCallbackReceived = false;
			PlayerAccount.SetCloudSaveData("{}", () => isCallbackReceived = true);

			await UniTask.WaitWhile(() => isCallbackReceived == false);

			Debug.Log("DATA DELETED");
		}
	}
}