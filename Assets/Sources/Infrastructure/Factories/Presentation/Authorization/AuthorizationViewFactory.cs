using System;
using Sources.BusinessLogic;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Infrastructure.Factories.Presentation.UI;
using Sources.Presentation.UI;
using Sources.Presentation.UI.YandexAuthorization;
using Sources.PresentationInterfaces;
using Sources.Utils;
using UnityEngine;

namespace Sources.Infrastructure.Factories.Presentation.Authorization
{
	public class AuthorizationViewFactory
	{
		private readonly IAssetFactory _assetFactory;
		private readonly TranslatorService _localizationService;
		private readonly IMainMenuView _mainMenuView;

		public AuthorizationViewFactory(
			IAssetFactory assetFactory,
			TranslatorService localizationService,
			IMainMenuView mainMenuView
		)
		{
			_assetFactory
				= assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
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

			return CreateYandexAuthorizationWindow();
#endif
			//return CreateEditorAuthorizationView();
		}

		private IAuthorizationView CreateYandexAuthorizationWindow(bool isEnabled = false)
		{
			YandexAuthorizationView view = _assetFactory.InstantiateAndGetComponent<YandexAuthorizationView>(
				YandexPath,
				_mainMenuView.Transform
			);

			view.gameObject.SetActive(isEnabled);

			TextPhrasesList phrasesList = view.GetComponent<TextPhrasesList>();
			view.Construct(view.GetComponent<RectTransform>(), null, phrasesList);
			view.TextPhrases.Phrases = _localizationService.GetLocalize(phrasesList.Phrases);

			return view;
		}

#if !YANDEX_CODE
		private IAuthorizationView CreateEditorAuthorizationView()
		{
			var view = _assetFactory.InstantiateAndGetComponent<EditorAuthorizationView>(
				EditorAuthorizationView,
				_mainMenuView.Transform
			);

			var phrases = view.GetComponent<TextPhrasesList>();
			view.Construct(view.GetComponent<RectTransform>(), null, phrases);
			view.TextPhrases.Phrases = _localizationService.GetLocalize(phrases.Phrases);
			return view;
		}
#endif
	}
}