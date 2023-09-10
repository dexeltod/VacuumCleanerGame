using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Joystick_Pack.Scripts.Base;
using Sources.ApplicationServicesInterfaces;
using Sources.DIService;
using Sources.DomainInterfaces;
using Sources.Services.Interfaces;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Services
{
	public class UIFactory : IUIFactory
	{
		private readonly IAssetProvider _assetProvider;
		private readonly IResourcesProgressPresenter _resourcesProgressPresenter;
		private readonly IPersistentProgressService _gameProgress;
		private GameplayInterfaceView _gameInterface;
		private readonly IYandexSDKController _yandexGamesController;
		public GameObject GameObject { get; private set; }
		public Canvas Canvas { get; private set; }
		public Slider ScoreSlider { get; private set; }
		private TextMeshProUGUI _playerName;
		public TextMeshProUGUI ScoreText { get; private set; }
		public TextMeshProUGUI MoneyText { get; private set; }
		public Joystick Joystick { get; private set; }

		public UIFactory()
		{
#if YANDEX_GAMES && !UNITY_EDITOR
			_yandexGamesController = GameServices.Container.Get<IYandexSDKController>();
#endif
			_assetProvider = GameServices.Container.Get<IAssetProvider>();
			_resourcesProgressPresenter = GameServices.Container.Get<IResourcesProgressPresenter>();
			_gameProgress = GameServices.Container.Get<IPersistentProgressService>();
		}

		public async UniTask<GameObject> CreateUI()
		{
			GameObject instance = _assetProvider.Instantiate(ResourcesAssetPath.Scene.UI.UI);
			_gameInterface = instance.GetComponent<GameplayInterfaceView>();

			_gameInterface.Construct(_resourcesProgressPresenter,
				_gameProgress.GameProgress.ResourcesModel.MaxFilledScore);

#if YANDEX_GAMES && !UNITY_EDITOR

			PlayerAccountProfileDataResponse playerAccount = await _yandexGamesController.GetPlayerAccount();

			string publicName = playerAccount.scopePermissions.public_name;
			
			_playerName = _gameInterface.PlayerName;
			_playerName.SetText(playerAccount.publicName);
#endif

			Canvas = _gameInterface.Canvas;

			GameObject = _gameInterface.gameObject;
			Joystick = _gameInterface.Joystick;
			ScoreSlider = _gameInterface.ScoreSlider;
			ScoreText = _gameInterface.ScoreText;
			MoneyText = _gameInterface.ScoreText;
			return instance;
		}
	}
}