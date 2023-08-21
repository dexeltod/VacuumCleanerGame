using Joystick_Pack.Scripts.Base;
using Sources.Application.Utils.Configs;
using Sources.DIService;
using Sources.Services.Interfaces;
using Sources.ServicesInterfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Services
{
	public class UIFactory : IUIFactory
	{
		private readonly IResourceProvider _assetProvider;
		private readonly IResourcesProgressViewModel _resourcesProgressViewModel;
		private readonly IPersistentProgressService _gameProgress;
		private GameplayInterfaceView _gameInterface;
		public GameObject GameObject { get; private set; }
		public Canvas Canvas { get; private set; }
		public Slider ScoreSlider { get; private set; }
		public TextMeshProUGUI ScoreText { get; private set; }
		public TextMeshProUGUI MoneyText { get; private set; }
		public Joystick Joystick { get; private set; }

		public UIFactory()
		{
			_assetProvider = GameServices.Container.Get<IResourceProvider>();
			_resourcesProgressViewModel = GameServices.Container.Get<IResourcesProgressViewModel>();
			_gameProgress = GameServices.Container.Get<IPersistentProgressService>();
		}

		public GameObject CreateUI()
		{
			GameObject instance = _assetProvider.Instantiate(ResourcesAssetPath.Scene.UI.UI);
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