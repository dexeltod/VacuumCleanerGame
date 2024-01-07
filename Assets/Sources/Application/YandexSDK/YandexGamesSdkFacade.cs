using System;
using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Sources.ApplicationServicesInterfaces;
using Sources.DomainInterfaces;
using UnityEngine;

namespace Sources.Application.YandexSDK
{
	public class YandexGamesSdkFacade : IYandexSDKController
	{
		private const int Delay = 1500;
		
		private readonly IYandexAuthorizationView _yandexAuthorizationView;
		private readonly IRewardService _rewardService;

		private PlayerAccountProfileDataResponse _playerAccount;
		private IGameProgressModel _gameProgress;

		private bool _isAuthorized;

		public YandexGamesSdkFacade(IYandexAuthorizationView yandexAuthorizationView, IRewardService rewardService)
		{
			_yandexAuthorizationView = yandexAuthorizationView;
			_rewardService = rewardService ?? throw new ArgumentNullException(nameof(rewardService));
		}

		public void SetStatusInitialized() =>
			YandexGamesSdk.GameReady();

		public async UniTask Initialize()
		{
			bool isInitialized = false;

			await YandexGamesSdk.Initialize(() => { isInitialized = true; });
			await UniTask.WaitWhile(() => isInitialized == false);

			if (PlayerAccount.IsAuthorized == false)
			{
				bool isNeedAuthorization = false;
				bool isProcessCompleted = false;

				_yandexAuthorizationView.IsWantsAuthorization(
					response => { isNeedAuthorization = response; },
					() => isProcessCompleted = true
				);

				await UniTask.WaitWhile(() => isProcessCompleted == false);

				if (isNeedAuthorization == false)
					return;

				isProcessCompleted = false;

				PlayerAccount.Authorize();
				PlayerAccount.StartAuthorizationPolling(
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
			(
				response =>
				{
					playerAccount = response;
					isReceived = true;
				}
			);

			await UniTask.WaitWhile(() => isReceived == false);

			Debug.Log("PLAYER ACCOUNT NAME : " + playerAccount.publicName);

			return playerAccount;
		}

		public async UniTask ShowAd(Action onOpenCallback, Action onRewardsCallback, Action onCloseCallback)
		{
			bool isClosed = false;
			bool isRewarded = false;

			VideoAd.Show(
				() => onOpenCallback?.Invoke(),
				() => isClosed = true,
				() => isRewarded = true
			);

			await UniTask.WaitWhile(() => isClosed == true || isRewarded == true);

			if (isClosed == true)
				onCloseCallback.Invoke();
			else if (isRewarded == true)
				onRewardsCallback.Invoke();
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

		public async UniTask DeleteSaves(IGameProgressModel gameProgressModel)
		{
			bool isCallbackReceived = false;
			PlayerAccount.SetCloudSaveData("{}", () => isCallbackReceived = true);

			await UniTask.WaitWhile(() => isCallbackReceived == false);

			Debug.Log("DATA DELETED");
		}
	}
}