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

		private readonly ISaveLoader _saveLoader;
		private readonly IPersistentProgressService _persistentProgressService;
		private readonly IShopItemFactory _shopFactory;
		private readonly IResourceService _resourceService;

		public ProgressFactory(ISaveLoader saveLoader,
			IPersistentProgressService persistentProgressService, IShopItemFactory shopItemFactory)
		{
			_saveLoader = saveLoader;
			_persistentProgressService = persistentProgressService;
			_shopFactory = shopItemFactory;
			_resourceService = GameServices.Container.Get<IResourceService>();
		}

		public async UniTask<IGameProgressModel> Load() =>
			await _saveLoader.Load();

		public void Save(IGameProgressModel model) =>
			_saveLoader.Save(model);

		public async UniTask InitProgress()
		{
			IGameProgressModel model = await _saveLoader.Load();
			Initialize(model);
		}

		private void Initialize(IGameProgressModel loadedProgress)
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

			_saveLoader.Save(newProgress);
			
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

		private Resource<int> GetResource(ResourceType type) =>
			_resourceService.GetResource<int>(type) as Resource<int>;
	}
}