using System;
using DG.Tweening;
using Sources.Presentation;
using Sources.Presentation.UI;
using Sources.Presentation.UI.YandexAuthorization;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;
using UnityEngine;

namespace Sources.Infrastructure.Factories.LeaderBoard
{
	public class AuthorizationViewFactory
	{
		private readonly IAssetFactory _assetFactory;
		private readonly ITranslatorService _localizationService;

		public AuthorizationViewFactory(IAssetFactory assetFactory, ITranslatorService localizationService)
		{
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
		}

		private string EditorAuthorizationView => ResourcesAssetPath.Scene.UIResources.Editor.AuthorizationView;
		private string YandexAuthorizationView => ResourcesAssetPath.Scene.UIResources.Yandex.YandexAuthorizationView;

		public IAuthorizationView Create()
		{
#if YANDEX_CODE
			return CreateYandexAuthorizationHandler();
#endif
			return CreateEditorAuthorizationView();
		}

		private IAuthorizationView CreateYandexAuthorizationHandler()
		{
			var view = _assetFactory.InstantiateAndGetComponent<YandexAuthorizationView>(YandexAuthorizationView);
			
			var phrases = view.GetComponent<TextPhrases>();
			view.Construct(view.GetComponent<RectTransform>(), null, phrases);
			view.TextPhrases.Phrases = _localizationService.Localize(phrases.Phrases);

			return view;
		}

		private IAuthorizationView CreateEditorAuthorizationView()
		{
			var view = _assetFactory.InstantiateAndGetComponent<EditorAuthorizationView>(EditorAuthorizationView);
			var phrases = view.GetComponent<TextPhrases>();
			view.Construct(view.GetComponent<RectTransform>(), null, phrases);
			view.TextPhrases.Phrases = _localizationService.Localize(phrases.Phrases);
			return view;
		}
	}
}