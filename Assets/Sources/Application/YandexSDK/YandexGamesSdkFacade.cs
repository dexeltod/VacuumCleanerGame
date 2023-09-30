using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Sources.ApplicationServicesInterfaces;
using Sources.DomainInterfaces;
using Sources.PresentationInterfaces;
using UnityEngine;

namespace Sources.Application.YandexSDK
{
	public class YandexGamesSdkFacade : IYandexSDKController
	{
		private const    int                         Delay = 1500;
		private readonly IYandexAuthorizationHandler _yandexAuthorizationHandler;

		private PlayerAccountProfileDataResponse _playerAccount;
		private IGameProgressModel               _gameProgress;

		private bool _isAuthorized;

		public YandexGamesSdkFacade(IYandexAuthorizationHandler yandexAuthorizationHandler)
		{
			_yandexAuthorizationHandler = yandexAuthorizationHandler;
		}

		public async UniTask Initialize()
		{
			bool isInitialized = false;

			await YandexGamesSdk.Initialize(() => { isInitialized = true; });
			await UniTask.WaitWhile(() => isInitialized == false);

			if (PlayerAccount.IsAuthorized == false)
			{
				bool isNeedAuthorization = false;
				bool isProcessCompleted  = false;

				Debug.Log("Waiting");

				_yandexAuthorizationHandler.IsWantsAuthorization
				(
					callback: isPlayerMadeChoice => isNeedAuthorization      = isPlayerMadeChoice,
					isPlayerWantsAuthorizeCallback: () => isProcessCompleted = true
				);

				await UniTask.WaitWhile(() => isProcessCompleted == false);

				if (isNeedAuthorization == false)
					return;

				isProcessCompleted = false;

				PlayerAccount.Authorize();
				PlayerAccount.StartAuthorizationPolling
				(
					Delay,
					() => { isProcessCompleted = true; }
				);

				await UniTask.WaitWhile(() => isProcessCompleted == false);
			}
		}

		public async UniTask<PlayerAccountProfileDataResponse> GetPlayerAccount()
		{
			if (PlayerAccount.IsAuthorized == false)
				return null;

			bool isReceived = false;

			PlayerAccountProfileDataResponse playerAccount = null;

			PlayerAccount.GetProfileData
			(response =>
				{
					playerAccount = response;
					isReceived    = true;
				}
			);

			await UniTask.WaitWhile(() => isReceived == false);

			Debug.Log("PLAYER ACCOUNT NAME : " + playerAccount.publicName);

			return playerAccount;
		}

		public async UniTask Save(string json)
		{
			bool isCallbackReceived = false;

			PlayerAccount.SetCloudSaveData(json, () => isCallbackReceived = true);
			await UniTask.WaitWhile(() => isCallbackReceived == false);
			Debug.Log("Player data saved" + json);
		}

		public async UniTask<string> Load()
		{
			bool   isReceived = false;
			string json       = "";

			PlayerAccount.GetCloudSaveData
			(
				successCallback =>
				{
					isReceived = true;
					json       = successCallback;
				},
				errorCallback =>
				{
					Debug.LogError(errorCallback);

					isReceived = true;
					json       = null;
				}
			);

			await UniTask.WaitWhile(() => isReceived == false);

			return json;
		}

		public async UniTask DeleteSaves(IGameProgressModel gameProgressModel)
		{
			bool isReceived = false;
			PlayerAccount.SetCloudSaveData("{}", () => isReceived = true);

			await UniTask.WaitWhile(() => isReceived == false);

			Debug.Log("DATA DELETED");
		}
	}
}