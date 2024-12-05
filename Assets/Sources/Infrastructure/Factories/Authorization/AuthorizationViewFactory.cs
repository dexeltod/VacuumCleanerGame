using System;
using Sources.Infrastructure.Factories.LeaderBoard;
using Sources.Presentation.UI;
using Sources.Presentation.UI.YandexAuthorization;
using Sources.PresentationInterfaces;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Sources.Utils;
using UnityEngine;

namespace Sources.Infrastructure.Factories.Authorization
{
    public class AuthorizationViewFactory
    {
        private readonly IAssetFactory _assetFactory;
        private readonly ITranslatorService _localizationService;
        private readonly MainMenuView _mainMenuView;

        public AuthorizationViewFactory(
            IAssetFactory assetFactory,
            ITranslatorService localizationService,
            MainMenuView mainMenuView
        )
        {
            _assetFactory
                = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
            _localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
            _mainMenuView = mainMenuView ?? throw new ArgumentNullException(nameof(mainMenuView));
        }

        private string EditorAuthorizationView => ResourcesAssetPath.Scene.UIResources.Editor.AuthorizationView;
        private string ViewPath => ResourcesAssetPath.Scene.UIResources.Yandex.YandexAuthorizationView;

        public IAuthorizationView Create(bool isDebug)
        {
#if YANDEX_CODE
            return CreateYandexAuthorizationWindow(isDebug);
#endif

            return CreateYandexAuthorizationWindow(isDebug);
            //return CreateEditorAuthorizationView();
        }

        private IAuthorizationView CreateYandexAuthorizationWindow(bool isEnabled = false)
        {
            YandexAuthorizationView view = _assetFactory.InstantiateAndGetComponent<YandexAuthorizationView>(
                ViewPath,
                _mainMenuView.transform
            );

            view.gameObject.SetActive(isEnabled);

            TextPhrases phrases = view.GetComponent<TextPhrases>();
            view.Construct(view.GetComponent<RectTransform>(), null, phrases);
            view.TextPhrases.Phrases = _localizationService.GetLocalize(phrases.Phrases);


            return view;
        }

        private IAuthorizationView CreateEditorAuthorizationView()
        {
            var view = _assetFactory.InstantiateAndGetComponent<EditorAuthorizationView>(
                EditorAuthorizationView,
                _mainMenuView.transform
            );

            var phrases = view.GetComponent<TextPhrases>();
            view.Construct(view.GetComponent<RectTransform>(), null, phrases);
            view.TextPhrases.Phrases = _localizationService.GetLocalize(phrases.Phrases);
            return view;
        }
    }
}