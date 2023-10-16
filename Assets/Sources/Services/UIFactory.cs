#if YANDEX_GAMES && !UNITY_EDITOR
using Agava.YandexGames;
#endif
using Cysharp.Threading.Tasks;
using Sources.DIService;
using Sources.DomainInterfaces;
using Sources.PresentationInterfaces;
using Sources.Services.Interfaces;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs;
using UnityEngine;

namespace Sources.Services
{
	public class UIFactory : IUIFactory
	{
		private readonly IAssetProvider                 _assetProvider;
		private readonly IResourcesProgressPresenter    _resourcesProgressPresenter;
		private readonly IPersistentProgressService     _gameProgress;

		public IGameplayInterfaceView GameplayInterface { get; private set; }

		public UIFactory()
		{
			
			_assetProvider                 = GameServices.Container.Get<IAssetProvider>();
			_resourcesProgressPresenter    = GameServices.Container.Get<IResourcesProgressPresenter>();
			_gameProgress                  = GameServices.Container.Get<IPersistentProgressService>();
		}

		public async UniTask<GameObject> CreateUI()
		{
			GameObject instance = _assetProvider.Instantiate(ResourcesAssetPath.Scene.UIResources.UI);
			GameplayInterface = instance.GetComponent<IGameplayInterfaceView>();

			GameplayInterface.Construct
			(
				_resourcesProgressPresenter,
				_gameProgress
					.GameProgress
					.ResourcesModel
					.MaxScore
			);

			return instance;
		}
	}
}