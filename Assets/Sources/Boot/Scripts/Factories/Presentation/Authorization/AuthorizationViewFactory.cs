using System;
using Sources.Boot.Scripts.Factories.Presentation.UI;
using Sources.BusinessLogic;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Presentation.UI;
using Sources.Presentation.UI.YandexAuthorization;
using Sources.PresentationInterfaces;
using Sources.Utils;
using UnityEngine;

namespace Sources.Boot.Scripts.Factories.Presentation.Authorization
{
	public class AuthorizationViewFactory
	{
		private readonly IAssetLoader _assetLoader;
		private readonly TranslatorService _localizationService;
		private readonly IMainMenuView _mainMenuView;

		public AuthorizationViewFactory(
			IAssetLoader assetLoader,
			TranslatorService localizationService,
			IMainMenuView mainMenuView
		)
		{
			_assetLoader
				= assetLoader ?? throw new ArgumentNullException(nameof(assetLoader));
			_localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
			_mainMenuView = mainMenuView ?? throw new ArgumentNullException(nameof(mainMenuView));
		}

		private string EditorAuthorizationView => ResourcesAssetPath.Scene.UIResources.Editor.AuthorizationView;
		private string YandexPath => ResourcesAssetPath.Scene.UIResources.Yandex.YandexAuthorizationView;

		public IAuthorizationView Create()
		{
#if YANDEX_CODE
			return CreateYandexAuthorizationWindow();
#endif
#if DEBUG

#endif
			return CreateEditorAuthorizationView();
		}

#if !YANDEX_CODE
		private IAuthorizationView CreateEditorAuthorizationView()
		{
			var view = _assetLoader.InstantiateAndGetComponent<EditorAuthorizationView>(
				EditorAuthorizationView,
				_mainMenuView.Transform
			);

			var phrases = view.GetComponent<TextPhrasesList>();
			view.Construct(view.GetComponent<RectTransform>(), null, phrases);
			view.TextPhrases.Phrases = _localizationService.GetLocalize(phrases.Phrases);
			return view;
		}
#endif

		private IAuthorizationView CreateYandexAuthorizationWindow(bool isEnabled = false)
		{
			var view = _assetLoader.InstantiateAndGetComponent<YandexAuthorizationView>(
				YandexPath,
				_mainMenuView.Transform
			);

			view.gameObject.SetActive(isEnabled);

			var phrasesList = view.GetComponent<TextPhrasesList>();
			view.Construct(view.GetComponent<RectTransform>(), null, phrasesList);
			view.TextPhrases.Phrases = _localizationService.GetLocalize(phrasesList.Phrases);

			return view;
		}
	}
}