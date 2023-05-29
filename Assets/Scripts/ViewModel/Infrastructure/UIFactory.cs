using System.Threading.Tasks;
using Model.DI;
using Model.Infrastructure.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using View.UI;
using ViewModel.Infrastructure.Services;
using ViewModel.Infrastructure.Services.Factories;

namespace ViewModel.Infrastructure
{
	public class UIFactory : IUIFactory
	{
		private const string UserInterface = "UI";
		private readonly IAssetProvider _assetProvider;
		private readonly IPlayerProgressViewModel _playerProgressViewModel;
		private readonly PlayerProgress _gameProgress;
		private GameplayInterfaceView _gameInterface;
		public GameObject GameObject { get; private set; }
		public Canvas Canvas { get; private set; }
		public Slider ScoreSlider { get; private set; }
		public TextMeshProUGUI ScoreText { get; private set; }
		public GameObject ProgressPanel { get; private set; }
		public TextMeshProUGUI MoneyText { get; private set; }
		public Joystick Joystick { get; private set; }

		public UIFactory()
		{
			_assetProvider = ServiceLocator.Container.GetSingle<IAssetProvider>();
			_playerProgressViewModel = ServiceLocator.Container.GetSingle<IPlayerProgressViewModel>();
			_gameProgress = ServiceLocator.Container.GetSingle<IPersistentProgressService>().GameProgress.PlayerProgress;
		}

		public async Task<GameObject> CreateUI()
		{
			GameObject instance = await _assetProvider.Instantiate(UserInterface);
			_gameInterface = instance.GetComponent<GameplayInterfaceView>();
			
			_gameInterface.Construct(_playerProgressViewModel, _gameProgress.MaxFilledScore);

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