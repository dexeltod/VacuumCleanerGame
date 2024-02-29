using System;
using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Sources.ApplicationServicesInterfaces;
using Sources.Infrastructure.Yandex;

namespace Sources.Infrastructure.StateMachine.GameStates
{
#if YANDEX_CODE
	public class YandexCloudPlayerDataAdapter : ICloudPlayerDataService
	{
		private readonly YandexServiceSdkFacade _yandexServiceSdkFacade;

		public YandexCloudPlayerDataAdapter(YandexServiceSdkFacade yandexServiceSdkFacade) =>
			_yandexServiceSdkFacade = yandexServiceSdkFacade ??
				throw new ArgumentNullException(nameof(yandexServiceSdkFacade));

		public async UniTask<IPlayerAccount> GetPlayerAccount()
		{
			PlayerAccountProfileDataResponse account = await _yandexServiceSdkFacade.GetPlayerAccount();

			return new PlayerAccount(
				account.publicName,
				account.uniqueID,
				account.lang,
				account.profilePicture
			);
		}

		public void SetStatusInitialized() =>
			_yandexServiceSdkFacade.SetStatusInitialized();

		public void Authorize() =>
			_yandexServiceSdkFacade.Authorize();
	}
#endif
}