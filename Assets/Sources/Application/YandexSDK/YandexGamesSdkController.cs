using System.Data;
using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Sources.ApplicationServicesInterfaces;
using Sources.DomainInterfaces;
using Sources.View.SceneEntity;
using UnityEngine;

namespace Sources.Application.YandexSDK
{
	public class YandexGamesSdkController : IYandexSDKController
	{
		private const int Delay = 1500;
		private readonly ICoroutineRunner _coroutineRunner;
		private readonly LoadingCurtain _loadingCurtain;

		private PlayerAccountProfileDataResponse _playerAccount;

		private bool _isAuthorized;
		private IGameProgressModel _gameProgress;

		public YandexGamesSdkController(ICoroutineRunner coroutineRunner, LoadingCurtain loadingCurtain)
		{
			_coroutineRunner = coroutineRunner;
			_loadingCurtain = loadingCurtain;
		}

		public async UniTask Initialize()
		{
			bool isInitialized = false;

			_loadingCurtain.SetText("Initialization SDK");
			
			_coroutineRunner.StartCoroutine
			(
				YandexGamesSdk.Initialize(() =>
					{
						isInitialized = true;
					}
				)
			);

			await UniTask.WaitWhile(() => isInitialized == false);

			if (PlayerAccount.IsAuthorized == false)
			{
				bool isAuthorized = false;

				_loadingCurtain.SetText("Starting authorization");

				PlayerAccount.StartAuthorizationPolling
				(
					Delay,
					() => { isAuthorized = true; }
				);

				await UniTask.WaitWhile(() => isAuthorized == false);

				_loadingCurtain.SetText("");
			}
		}

		public async UniTask<PlayerAccountProfileDataResponse> GetPlayerAccount()
		{
			bool isReceived = false;
			
			PlayerAccountProfileDataResponse dataResponse = null;

			_loadingCurtain.SetText("Getting player account");

			PlayerAccount.GetProfileData
			(response =>
				{
					dataResponse = response;
					isReceived = true;
				}
			);

			await UniTask.WaitWhile(() => isReceived == false);

			return dataResponse;
		}

		public void Save(string json) =>
			PlayerAccount.SetCloudSaveData(json);

		public async UniTask<string> Load()
		{
			bool isReceived = false;
			string json = "";

			_loadingCurtain.SetText("Getting player saves");

			PlayerAccount.GetCloudSaveData
			(
				successCallback =>
				{
					json = successCallback;
					isReceived = true;
				},
				errorCallback =>
				{
					json = errorCallback;
					isReceived = false;
				}
			);

			await UniTask.WaitWhile(() => isReceived == false);

			return json;
		}
	}
}