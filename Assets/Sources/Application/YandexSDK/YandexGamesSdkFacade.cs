using System;
using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Sources.ApplicationServicesInterfaces;
using Sources.DomainInterfaces;
using Sources.ServicesInterfaces.Authorization;
using UnityEngine;
using VContainer;

namespace Sources.Application.YandexSDK
{
#if YANDEX_CODE
	public class YandexGamesSdkFacade : IYandexSDKController
	{
		private const int Delay = 1500;

		private readonly IAuthorization _authorizationView;

		private PlayerAccountProfileDataResponse _playerAccount;
		private IGameProgressModel _gameProgress;

		private bool _isAuthorized;

		[Inject]
		public YandexGamesSdkFacade(IAuthorization authorizationView) =>
			_authorizationView = authorizationView ??
				throw new ArgumentNullException(nameof(authorizationView));

		public void SetStatusInitialized() =>
			YandexGamesSdk.GameReady();

		public async UniTask Initialize()
		{
			bool isInitialized = false;
			await YandexGamesSdk.Initialize(() => { isInitialized = true; });
		}

		public async UniTask Authorize()
		{
			if (PlayerAccount.IsAuthorized == false)
			{
				_authorizationView.EnableAuthorizeWindow();
				_authorizationView.AuthorizeCallback += OnAuthorize;
			}
		}

		private async void OnAuthorize(bool isAuthorize)
		{
			if (isAuthorize == false)
			{
				_authorizationView.AuthorizeCallback -= OnAuthorize;
				_authorizationView.DisableAuthorizeWindow();
				return;
			}

			bool isProcessCompleted = false;

			PlayerAccount.Authorize();
			PlayerAccount.StartAuthorizationPolling(
				Delay,
				() => isProcessCompleted = true
			);

			await UniTask.WaitWhile(() => isProcessCompleted == false);
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
	}
#endif
}