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
                _mainMenuView.transform
            );

            view.gameObject.SetActive(isEnabled);

            TextPhrasesList phrasesList = view.GetComponent<TextPhrasesList>();
            view.Construct(view.GetComponent<RectTransform>(), null, phrasesList);
            view.TextPhrases.Phrases = _localizationService.GetLocalize(phrasesList.Phrases);


            return view;
        }

        private IAuthorizationView CreateEditorAuthorizationView()
        {
            var view = _assetFactory.InstantiateAndGetComponent<EditorAuthorizationView>(
                EditorAuthorizationView,
                _mainMenuView.transform
            );

            var phrases = view.GetComponent<TextPhrasesList>();
            view.Construct(view.GetComponent<RectTransform>(), null, phrases);
            view.TextPhrases.Phrases = _localizationService.GetLocalize(phrases.Phrases);
            return view;
        }
    }
}