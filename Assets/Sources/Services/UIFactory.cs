#if YANDEX_GAMES && !UNITY_EDITOR
using Agava.YandexGames;
#endif

using System;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces.Factory;
using Sources.PresentationInterfaces;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;
using VContainer;

namespace Sources.Services
{
	public class UIFactory : IUIFactory
	{
		private const bool IsActiveOnStart = true;

		private readonly IAssetResolver _assetResolver;
		private readonly IResourceProgressEventHandler _resourceProgressEventHandler;
		private readonly IPersistentProgressService _gameProgress;
		private readonly ITranslatorService _translatorService;

		private readonly string _uiResourcesUI = ResourcesAssetPath.Scene.UIResources.UI;

		public IGameplayInterfaceView GameplayInterface { get; private set; }

		public UIFactory(
			IAssetResolver assetResolver,
			IResourceProgressEventHandler resourceProgressEventHandler,
			IPersistentProgressService persistentProgressService,
			 ITranslatorService translatorService
		)
		{
			_assetResolver = assetResolver ?? throw new ArgumentNullException(nameof(assetResolver));

			_resourceProgressEventHandler = resourceProgressEventHandler ??
				throw new ArgumentNullException(nameof(resourceProgressEventHandler));

			_gameProgress = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
		}

		public IGameplayInterfaceView Instantiate()
		{
			IGameplayInterfaceView gameplayInterfaceView = Load();
			IResourcesModel model = GetModel();
			Construct(model);

			return gameplayInterfaceView;
		}

		public void SetActive(bool isActive) =>
			GameplayInterface.GameObject.SetActive(isActive);

		private void Construct(IResourcesModel model)
		{
			if (model == null) throw new ArgumentNullException(nameof(model));

			GameplayInterface.Construct(
				model.CurrentCashScore,
				model.MaxCashScore,
				model.MaxGlobalScore,
				model.SoftCurrency.Count,
				_resourceProgressEventHandler,
				IsActiveOnStart
			);

			GameplayInterface.Phrases.Phrases = _translatorService.Localize(GameplayInterface.Phrases.Phrases);
		}

		private IResourcesModel GetModel() =>
			_gameProgress
				.GameProgress
				.ResourcesModel;

		private IGameplayInterfaceView Load() =>
			GameplayInterface = _assetResolver
				.Instantiate(_uiResourcesUI)
				.GetComponent<IGameplayInterfaceView>();
	}
}