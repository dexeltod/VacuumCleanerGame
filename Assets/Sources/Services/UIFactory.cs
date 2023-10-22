#if YANDEX_GAMES && !UNITY_EDITOR
using Agava.YandexGames;
#endif

using Sources.DomainInterfaces;
using Sources.PresentationInterfaces;
using Sources.Services.Interfaces;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs;

namespace Sources.Services
{
	public class UIFactory : IUIFactory
	{
		private readonly IAssetProvider                _assetProvider;
		private readonly IResourceProgressEventHandler _resourceProgressEventHandler;
		private readonly IPersistentProgressService    _gameProgress;

		public IGameplayInterfaceView GameplayInterface { get; private set; }

		public UIFactory
		(
			IAssetProvider                assetProvider,
			IResourceProgressEventHandler resourceProgressEventHandler,
			IPersistentProgressService    persistentProgressService
		)
		{
			_assetProvider                = assetProvider;
			_resourceProgressEventHandler = resourceProgressEventHandler;
			_gameProgress                 = persistentProgressService;
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

		private void Construct(IResourcesModel model) =>
			GameplayInterface.Construct
			(
				model.MaxCashScore,
				model.SoftCurrency.Count,
				_resourceProgressEventHandler
			);

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