using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Sources.ApplicationServicesInterfaces;
using Sources.DomainInterfaces;
using Sources.PresentationInterfaces;
using Sources.View.SceneEntity;
using UnityEngine;

namespace Sources.Application.YandexSDK
{
	public class YandexGamesSdkFacade : IYandexSDKController
	{
		private const int Delay = 1500;
		private readonly LoadingCurtain _loadingCurtain;
		private readonly IYandexAuthorizationHandler _yandexAuthorizationHandler;

		private PlayerAccountProfileDataResponse _playerAccount;
		private IGameProgressModel _gameProgress;

		private bool _isAuthorized;

		public YandexGamesSdkFacade(LoadingCurtain loadingCurtain,
			IYandexAuthorizationHandler yandexAuthorizationHandler)
		{
			_loadingCurtain = loadingCurtain;
			_yandexAuthorizationHandler = yandexAuthorizationHandler;
		}

		public async UniTask Initialize()
		{
			bool isInitialized = false;

			_loadingCurtain.SetText("Initialization SDK");

			await YandexGamesSdk.Initialize(() => { isInitialized = true; });
			await UniTask.WaitWhile(() => isInitialized == false);

			if (PlayerAccount.IsAuthorized == false)
			{
				bool isNeedAuthorization = false;
				bool isProcessCompleted = false;
			
				Debug.Log("Waiting for choice");
				_yandexAuthorizationHandler.IsWantsAuthorization(isPlayerMadeChoice =>
					isNeedAuthorization = isPlayerMadeChoice);
			
				if (isNeedAuthorization == false)
					return;
			
				_loadingCurtain.SetText("Starting authorization");
			
				PlayerAccount.Authorize();
				PlayerAccount.StartAuthorizationPolling
				(
					Delay,
					() => { isProcessCompleted = true; }
				);
			
				await UniTask.WaitWhile(() => isProcessCompleted == false);
			
				_loadingCurtain.SetText("");
			}
		}

		public async UniTask<PlayerAccountProfileDataResponse> GetPlayerAccount()
		{
			if (PlayerAccount.IsAuthorized == false)
				return null;

			bool isReceived = false;

			PlayerAccountProfileDataResponse playerAccount = null;

			_loadingCurtain.SetText("Getting player account");

			PlayerAccount.GetProfileData
			(response =>
				{
					playerAccount = response;
					isReceived = true;
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
			bool isReceived = false;
			string json = "";

			_loadingCurtain.SetText("Getting player saves");

			PlayerAccount.GetCloudSaveData
			(
				successCallback =>
				{
					isReceived = true;
					json = successCallback;
				},
				errorCallback =>
				{
					Debug.LogError(errorCallback);

					isReceived = true;
					json = null;
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