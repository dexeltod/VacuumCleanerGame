using System;
using System.Threading.Tasks;
using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Yandex;
using Sources.InfrastructureInterfaces;
using UnityEngine;
using PlayerAccount = Sources.Domain.PlayerAccount;

namespace Sources.Infrastructure.Adapters
{
#if YANDEX_CODE
	public class YandexCloudAdapter : ICloudServiceSdkFacade
	{
		private readonly YandexServiceSdkFacade _yandexServiceSdkFacade;
		private PlayerAccount _playerAccount;
		private string _playerLanguage;

		public YandexCloudAdapter(YandexServiceSdkFacade yandexServiceSdkFacade) =>
			_yandexServiceSdkFacade = yandexServiceSdkFacade ??
				throw new ArgumentNullException(nameof(yandexServiceSdkFacade));

		public async UniTask<IPlayerAccount> GetPlayerAccount()
		{
			if (_playerAccount != null)
				return _playerAccount;

			PlayerAccountProfileDataResponse account = await _yandexServiceSdkFacade.GetPlayerAccount();

			_playerAccount = new PlayerAccount(
				account.publicName,
				account.uniqueID,
				account.lang,
				account.profilePicture
			);

			return _playerAccount;
		}

		public void SetStatusInitialized() =>
			_yandexServiceSdkFacade.SetStatusInitialized();

		public async UniTask Authorize() =>
			await _yandexServiceSdkFacade.Authorize();

		public bool IsAuthorized => _yandexServiceSdkFacade.IsAuthorized;

		public async UniTask<string> GetPlayerLanguage()
		{
			Debug.Log($"PlayerLanguage is {_playerAccount.Language}");

			_playerLanguage ??= await _yandexServiceSdkFacade.GetPlayerLanguage();

			return _playerLanguage;
		}
	}
#endif
}