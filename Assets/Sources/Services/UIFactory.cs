using System.Threading.Tasks;
using Sources.Core.DI;
using Sources.DomainServices.Interfaces;
using Sources.Infrastructure.InfrastructureInterfaces;
using Sources.Infrastructure.Services.Interfaces;
using Sources.View.Services.UI;
using Sources.View.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Infrastructure.Services
{
	public class UIFactory : IUIFactory
	{
		private const string UserInterface = "UI";
		private readonly IAssetProvider _assetProvider;
		private readonly IResourcesProgressViewModel _resourcesProgressViewModel;
		private readonly IPersistentProgressService _gameProgress;
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
			_assetProvider = ServiceLocator.Container.Get<IAssetProvider>();
			_resourcesProgressViewModel = ServiceLocator.Container.Get<IResourcesProgressViewModel>();
			_gameProgress = ServiceLocator.Container.Get<IPersistentProgressService>();
		}

		public async Task<GameObject> CreateUI()
		{
			GameObject instance = await _assetProvider.Instantiate(UserInterface);
			_gameInterface = instance.GetComponent<GameplayInterfaceView>();
			
			_gameInterface.Construct(_resourcesProgressViewModel, _gameProgress.GameProgress.ResourcesData.MaxFilledScore);

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