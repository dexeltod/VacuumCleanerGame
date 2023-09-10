using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Sources.ApplicationServicesInterfaces;
using Sources.View.SceneEntity;

namespace Sources.Application.YandexSDK
{
	public class YandexGamesSdkController : IYandexSDKController
	{
		private const int AuthorizationDelay = 1500;
		private readonly ICoroutineRunner _coroutineRunner;
		private readonly LoadingCurtain _loadingCurtain;

		private PlayerAccountProfileDataResponse _playerAccount;

		private bool _isInitialized;
		private bool _isAuthorized;
		private bool _isPlayerAccountReceived;

		public YandexGamesSdkController(ICoroutineRunner coroutineRunner, LoadingCurtain loadingCurtain)
		{
			_coroutineRunner = coroutineRunner;
			_loadingCurtain = loadingCurtain;
		}

		public async UniTask Initialize()
		{
			_loadingCurtain.SetText("Initialization SDK");
			_coroutineRunner.StartCoroutine(YandexGamesSdk.Initialize(OnInitialized));
			await UniTask.WaitWhile(() => _isInitialized == false);

			// if (PlayerAccount.IsAuthorized == false)
			// {
			// 	_loadingCurtain.SetText("Starting authorization");
			// 	PlayerAccount.StartAuthorizationPolling(AuthorizationDelay, OnAuthorizationEnded);
			// 	await UniTask.WaitWhile(() => _isAuthorized == false);
			// 	_loadingCurtain.SetText("");
			// }
		}

		public async UniTask<PlayerAccountProfileDataResponse> GetPlayerAccount()
		{
			_isPlayerAccountReceived = false;
			PlayerAccount.GetProfileData(OnPlayerAccountReceived);
			await UniTask.WaitWhile(() => _isPlayerAccountReceived == false);
			return _playerAccount;
		}

		private void OnPlayerAccountReceived(PlayerAccountProfileDataResponse playerAccount)
		{
			_playerAccount = playerAccount;
			_isPlayerAccountReceived = true;
		}

		private void OnAuthorizationEnded() =>
			_isAuthorized = true;

		private void OnInitialized() =>
			_isInitialized = true;
	}
}