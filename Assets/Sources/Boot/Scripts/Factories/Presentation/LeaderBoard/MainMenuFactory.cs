using System;
using Sources.BusinessLogic;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Presentation.UI;
using Sources.PresentationInterfaces;
using Sources.Utils;
using UnityEngine;

namespace Sources.Boot.Scripts.Factories.Presentation.LeaderBoard
{
	public class MainMenuFactory : IMainMenuFactory
	{
		private readonly IAssetLoader _assetLoader;
		private readonly TranslatorService _translatorService;

		private MainMenuView _mainMenuView;

		public MainMenuFactory(
			IAssetLoader assetLoader,
			TranslatorService translatorService
		)
		{
			_assetLoader
				= assetLoader ?? throw new ArgumentNullException(nameof(assetLoader));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
		}

		private string MainMenuCanvasResourcePath => ResourcesAssetPath.Scene.UIResources.MainMenuCanvas;

		public IMainMenuView Create()
		{
			GameObject gameObject = _assetLoader.Instantiate(MainMenuCanvasResourcePath);

			_mainMenuView = gameObject.GetComponent<MainMenuView>();

			_mainMenuView.Translator.Phrases = _translatorService.GetLocalize(_mainMenuView.Translator.Phrases);

			return _mainMenuView;
		}
	}
}