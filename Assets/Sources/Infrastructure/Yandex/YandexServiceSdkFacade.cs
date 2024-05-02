using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces;
using UnityEngine;

namespace Sources.Infrastructure.Yandex
{
#if YANDEX_CODE
	public class YandexServiceSdkFacade
	{
		private const int Delay = 1500;

		private PlayerAccountProfileDataResponse _playerAccount;
		private IGlobalProgress _gameProgress;

		private bool _isAuthorized;

		public void SetStatusInitialized() =>
			YandexGamesSdk.GameReady();

		public async void Authorize()
		{
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