#if YANDEX_GAMES && !UNITY_EDITOR
using Agava.YandexGames;
#endif

using System;
using Sources.DomainInterfaces;
using Sources.PresentationInterfaces;
using Sources.Services.Interfaces;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;

namespace Sources.Services
{
	public class UIFactory : IUIFactory
	{
		private readonly IAssetProvider _assetProvider;
		private readonly IResourceProgressEventHandler _resourceProgressEventHandler;
		private readonly IPersistentProgressService _gameProgress;

		private bool _isActiveOnStart = true;

		public IGameplayInterfaceView GameplayInterface { get; private set; }

		public UIFactory(
			IAssetProvider assetProvider,
			IResourceProgressEventHandler resourceProgressEventHandler,
			IPersistentProgressService persistentProgressService
		)
		{
			_assetProvider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));

			_resourceProgressEventHandler = resourceProgressEventHandler ??
				throw new ArgumentNullException(nameof(resourceProgressEventHandler));

			_gameProgress = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));
		}

		public IGameplayInterfaceView Instantiate() =>
			Create();

		public void SetActive(bool isActive) =>
			GameplayInterface.GameObject.SetActive(isActive);

		private IGameplayInterfaceView Create()
		{
			LoadAndSet();
			IResourcesModel model = GetModel();
			Construct(model);

			return GameplayInterface;
		}

		private void Construct(IResourcesModel model)
		{
			if (model == null) throw new ArgumentNullException(nameof(model));

			GameplayInterface.Construct(
				model.CurrentCashScore,
				model.MaxCashScore,
				model.MaxGlobalScore,
				model.SoftCurrency.Count,
				_resourceProgressEventHandler,
				_isActiveOnStart
			);
		}

		private IResourcesModel GetModel() =>
			_gameProgress
				.GameProgress
				.ResourcesModel;

		private void LoadAndSet() =>
			GameplayInterface = _assetProvider
				.Instantiate
				(
					ResourcesAssetPath
						.Scene
						.UIResources
						.UI
				)
				.GetComponent<IGameplayInterfaceView>();
	}
}