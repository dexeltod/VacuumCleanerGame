using System;
using System.Collections.Generic;
using Sources.BuisenessLogic;
using Sources.BuisenessLogic.ServicesInterfaces;
using Sources.Presentation.UI;
using Sources.Utils;
using UnityEngine;

namespace Sources.Presentation.Factories.LeaderBoard
{
	public class MainMenuFactory
	{
		private readonly IAssetFactory _assetFactory;
		private readonly TranslatorService _translatorService;

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
		private List<string> Phrases => _mainMenuView.Translator.Phrases;
		private string MainMenuCanvasResourcePath => ResourcesAssetPath.Scene.UIResources.MainMenuCanvas;

		public MainMenuView Create()
		{
			GameObject gameObject = _assetFactory.Instantiate(MainMenuCanvasResourcePath);

			_mainMenuView = gameObject.GetComponent<MainMenuView>();

			_mainMenuView.Translator.Phrases = _translatorService.GetLocalize(_mainMenuView.Translator.Phrases);

			return _mainMenuView;
		}
	}
}