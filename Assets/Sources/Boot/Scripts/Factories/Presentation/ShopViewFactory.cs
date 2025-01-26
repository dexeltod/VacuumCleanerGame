using System;
using System.Collections.Generic;
using System.Linq;
using Sources.BusinessLogic;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.DomainInterfaces.Models.Shop.Upgrades;
using Sources.Infrastructure.Repository;
using Sources.InfrastructureInterfaces.Configs;
using Sources.Presentation.UI.Shop;
using Sources.PresentationInterfaces;
using Sources.Utils;
using UnityEngine;

namespace Sources.Boot.Scripts.Factories.Presentation
{
	public class ShopViewFactory
	{
		private readonly IAssetLoader _assetLoader;
		private readonly IProgressEntityRepository _entityRepository;
		private readonly IGameplayInterfacePresenter _gameplayInterfaceProvider;
		private readonly IPersistentProgressService _persistentProgressServiceProvider;
		private readonly IPlayerModelRepository _playerModelRepository;
		private readonly IProgressEntityRepository _progressEntityRepository;
		private readonly IResourceModel _resourceModel;
		private readonly ISaveLoader _saveLoaderProvider;
		private readonly TranslatorService _translatorService;

		public ShopViewFactory(
			IPersistentProgressService persistentProgressService,
			TranslatorService translatorService,
			IProgressEntityRepository progressEntityRepository,
			IPlayerModelRepository playerModelRepositoryProvider,
			ISaveLoader saveLoaderProvider,
			IAssetLoader assetLoader,
			IProgressEntityRepository entityRepository,
			IResourceModel resourceModel
		)
		{
			_progressEntityRepository =
				progressEntityRepository ?? throw new ArgumentNullException(nameof(progressEntityRepository));
			_playerModelRepository = playerModelRepositoryProvider
			                         ?? throw new ArgumentNullException(nameof(playerModelRepositoryProvider));
			_saveLoaderProvider = saveLoaderProvider ?? throw new ArgumentNullException(nameof(saveLoaderProvider));
			_assetLoader = assetLoader ?? throw new ArgumentNullException(nameof(assetLoader));
			_entityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
			_resourceModel = resourceModel;
			_persistentProgressServiceProvider =
				persistentProgressService ?? throw new ArgumentNullException(nameof(persistentProgressService));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
		}

		public IEnumerable<IUpgradeElementPrefabView> Create(Transform transform, AudioSource audioSource)
		{
			IReadOnlyList<IUpgradeEntityConfig> configs = _entityRepository.GetConfigs();
			IReadOnlyList<IStatUpgradeEntityReadOnly> entities = _entityRepository.GetEntities();

			Dictionary<int, UpgradeElementPrefabView> a = configs.ToDictionary(
				elem => elem.Id,
				elem2 => elem2.PrefabView.GetComponent<UpgradeElementPrefabView>()
			);

			IEnumerable<UpgradeElementPrefabView> views = configs.Join(
				entities,
				elem => elem.Id,
				elem2 => elem2.ConfigId,
				(elem, elem2) =>
				{
					var view = _assetLoader.InstantiateAndGetComponent<UpgradeElementPrefabView>(elem.PrefabView, transform);

					view.Construct(
						elem.Icon,
						Localize(elem.Title),
						Localize(elem.Description),
						elem.Id,
						elem2.Value,
						_progressEntityRepository.GetPrice(elem.Id),
						elem.MaxProgressCount
					);

					return view;
				}
			);

			UpgradeElementPresenter presenter = CreateUpgradeElementPresenter(audioSource, views);
			return views;
		}

		private UpgradeElementPresenter CreateUpgradeElementPresenter(
			AudioSource audioSource,
			IEnumerable<IUpgradeElementChangeableView> views) =>
			new(
				views,
				_persistentProgressServiceProvider,
				_progressEntityRepository,
				_saveLoaderProvider,
				_assetLoader.LoadFromResources<AudioClip>(ResourcesAssetPath.SoundNames.SoundBuy),
				_assetLoader.LoadFromResources<AudioClip>(ResourcesAssetPath.SoundNames.SoundClose),
				audioSource,
				new ShopService(_progressEntityRepository, _playerModelRepository, _resourceModel)
			);

		private string Localize(string phrase) => _translatorService.GetLocalize(phrase);
	}
}
