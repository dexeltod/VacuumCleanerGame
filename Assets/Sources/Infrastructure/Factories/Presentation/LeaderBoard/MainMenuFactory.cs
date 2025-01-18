using System;
using Sources.BusinessLogic;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Presentation.UI;
using Sources.PresentationInterfaces;
using Sources.PresentationInterfaces.Factories;
using Sources.Utils;
using UnityEngine;
using VContainer;

namespace Sources.Presentation.Factories.LeaderBoard
{
	public class MainMenuFactory : IMainMenuFactory
	{
		private readonly IAssetFactory _assetFactory;
		private readonly TranslatorService _translatorService;

		[Inject]
		public MainMenuFactory(
			IAssetFactory assetFactory,
			TranslatorService translatorService
		)
		{
			_assetFactory
				= assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
		}

		private MainMenuView _mainMenuView;
		private string MainMenuCanvasResourcePath => ResourcesAssetPath.Scene.UIResources.MainMenuCanvas;

		public IMainMenuView Create()
		{
			GameObject gameObject = _assetFactory.Instantiate(MainMenuCanvasResourcePath);

			_mainMenuView = gameObject.GetComponent<MainMenuView>();

			_mainMenuView.Translator.Phrases = _translatorService.GetLocalize(_mainMenuView.Translator.Phrases);

			return _mainMenuView;
		}
	}
}