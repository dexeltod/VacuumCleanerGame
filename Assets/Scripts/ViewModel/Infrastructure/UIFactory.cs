using System.Threading.Tasks;
using DefaultNamespace.Presenter;
using Model.DI;
using Model.Infrastructure.Data;
using Model.Infrastructure.Services;
using Model.Infrastructure.Services.Factories;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ViewModel.Infrastructure
{
	public class UIFactory : IUIFactory
	{
		private const string UserInterface = "UI";
		private readonly IAssetProvider _assetProvider;
		private readonly IGameProgressViewModel _gameProgressViewModel;
		private readonly GameProgressModel _gameProgress;
		private GameplayInterfaceView _gameInterface;
		public GameObject This { get; private set; }
		public Slider ScoreSlider { get; private set; }
		public TextMeshProUGUI ScoreText { get; private set; }
		public GameObject ProgressPanel { get; private set; }
		public TextMeshProUGUI MoneyText { get; private set; }
		public Joystick Joystick { get; private set; }

		public UIFactory()
		{
			_assetProvider = ServiceLocator.Container.GetSingle<IAssetProvider>();
			_gameProgressViewModel = ServiceLocator.Container.GetSingle<IGameProgressViewModel>();
			_gameProgress = ServiceLocator.Container.GetSingle<IPersistentProgressService>().GameProgress;
		}

		public async Task<GameObject> CreateUI()
		{
			GameObject instance = await _assetProvider.Instantiate(UserInterface);
			_gameInterface = instance.GetComponent<GameplayInterfaceView>();
			
			_gameInterface.Construct(_gameProgressViewModel, _gameProgress.MaxFilledScore);

			This = _gameInterface.gameObject;
			Joystick = _gameInterface.Joystick;
			ScoreSlider = _gameInterface.ScoreSlider;
			ScoreText = _gameInterface.ScoreText;
			MoneyText = _gameInterface.ScoreText;
			return instance;
		}

	}
}