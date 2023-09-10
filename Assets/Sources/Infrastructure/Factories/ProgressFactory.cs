using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sources.DIService;
using Sources.Domain.Progress;
using Sources.Domain.Progress.Player;
using Sources.Domain.Progress.ResourcesData;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Upgrade;
using Sources.Utils;

namespace Sources.Infrastructure.Factories
{
	[Serializable]
	public class ProgressFactory
	{
		private const int StartMoneyCount = 9999;

		private readonly ISaveLoadDataService _saveLoadService;
		private readonly IPersistentProgressService _persistentProgressService;
		private readonly IShopItemFactory _shopFactory;
		private readonly IResourceService _resourceService;

		public ProgressFactory(ISaveLoadDataService saveLoadService,
			IPersistentProgressService persistentProgressService, IShopItemFactory shopItemFactory)
		{
			_saveLoadService = saveLoadService;
			_persistentProgressService = persistentProgressService;
			_shopFactory = shopItemFactory;
			_resourceService = GameServices.Container.Get<IResourceService>();
		}

		public async UniTask InitProgress()
		{
#if YANDEX_GAMES && !TEST_BUILD
			IGameProgressModel yandexProgress = await _saveLoadService.LoadFromYandex();
			Init(yandexProgress);
#endif

#if !YANDEX_GAMES
			IGameProgressModel loadedProgress = await _saveLoadService.LoadFromUnityCloud();
			Init(loadedProgress);
#endif

#if !YANDEX_GAMES && UNITY_EDITOR
			IGameProgressModel testProgress = CreateNewProgress();
			Init(testProgress);
#endif

#if TEST_BUILD
			IGameProgressModel testProgress = CreateNewProgress();
			Init(testProgress);
#endif
		}

		private void Init(IGameProgressModel loadedProgress)
		{
			loadedProgress = CreatNewIfNull(loadedProgress);
			_persistentProgressService.Construct(loadedProgress);
		}

		private IGameProgressModel CreatNewIfNull(IGameProgressModel loadedProgress)
		{
			if (loadedProgress == null)
			{
				IGameProgressModel newProgress = CreateNewProgress();
				loadedProgress = newProgress;
			}

			return loadedProgress;
		}

		private GameProgressModel CreateNewProgress()
		{
			IUpgradeItemData[] itemsList = _shopFactory.LoadItems();
			GameProgressModel newProgress = CreateProgress(itemsList);

			_persistentProgressService.Construct(newProgress);
#if !YANDEX_GAMES
			_saveLoadService.SaveToUnityCloud();
#endif

			return newProgress;
		}

		private GameProgressModel CreateProgress(IUpgradeItemData[] itemsList)
		{
			Resource<int> soft = GetResource(ResourceType.Soft);
			Resource<int> hard = GetResource(ResourceType.Hard);

			ResourcesModel resourcesModel = new ResourcesModel
			(
				soft,
				hard,
				StartMoneyCount
			);

			PlayerProgress playerProgressModel = new PlayerProgress(CreateNewUpgradeProgressData(itemsList));

			ShopProgress shopProgressModel = new(CreateNewUpgradeProgressData(itemsList));

			GameProgressModel newProgress = new GameProgressModel
			(
				resourcesModel,
				playerProgressModel,
				shopProgressModel
			);

			return newProgress;
		}

		private Resource<int> GetResource(ResourceType type) =>
			_resourceService.GetResource<int>(type) as Resource<int>;

		private List<ProgressUpgradeData> CreateNewUpgradeProgressData(IUpgradeItemData[] itemsList)
		{
			List<ProgressUpgradeData> progressList = new List<ProgressUpgradeData>();

			foreach (IUpgradeItemData itemData in itemsList)
			{
				progressList.Add(new ProgressUpgradeData(itemData.IdName, 0));
				itemData.SetUpgradeLevel(0);
			}

			return progressList;
		}
	}
}