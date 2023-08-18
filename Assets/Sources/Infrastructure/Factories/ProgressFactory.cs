using System.Collections.Generic;
using Sources.Application.Utils;
using Sources.Domain.Progress;
using Sources.Domain.Progress.Player;
using Sources.Domain.Progress.ResourcesData;
using Sources.DomainInterfaces;
using Sources.Infrastructure.ScriptableObjects;
using Sources.InfrastructureInterfaces;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.UI;

namespace Sources.Infrastructure.Factories
{
	public class ProgressFactory
	{
		private const int StartMoneyCount = 1000;
		private const string SpeedProgressName = "Speed";

		private readonly ISaveLoadDataService _saveLoadService;
		private readonly IPersistentProgressService _persistentProgressService;
		private readonly IShopItemFactory _shopFactory;

		public ProgressFactory(ISaveLoadDataService saveLoadService,
			IPersistentProgressService persistentProgressService, IShopItemFactory shopItemFactory)
		{
			_saveLoadService = saveLoadService;
			_persistentProgressService = persistentProgressService;
			_shopFactory = shopItemFactory;
		}

		public void InitProgress()
		{
			IGameProgressModel loadedProgress = _saveLoadService.LoadProgress();
			Init(loadedProgress);
		}

		private void Init(IGameProgressModel loadedProgress)
		{
			if (loadedProgress == null)
			{
				var newProgress = CreateNewProgress();
				loadedProgress = newProgress;
			}

			_persistentProgressService.Construct(loadedProgress);
		}

		private GameProgressModel CreateNewProgress()
		{
			IUpgradeItemData[] itemsList =  _shopFactory.LoadItems();

			GameProgressModel newProgress = CreateProgress(itemsList);

			_persistentProgressService.Construct(newProgress);
			_saveLoadService.SaveProgress();

			return newProgress;
		}

		private GameProgressModel CreateProgress(IUpgradeItemData[] itemsList)
		{
			ResourcesData resourcesData = new ResourcesData(new Resource(ResourceType.Soft), StartMoneyCount);

			PlayerProgress playerProgressModel = new PlayerProgress(CreateNewUpgradeProgressData(itemsList));

			ShopProgress shopProgressModel = new(CreateNewUpgradeProgressData(itemsList), new List<int>());

			GameProgressModel newProgress = new GameProgressModel
			(
				resourcesData,
				playerProgressModel,
				shopProgressModel
			);

			return newProgress;
		}

		private List<IUpgradeProgressData> CreateNewUpgradeProgressData(IUpgradeItemData[] itemsList)
		{
			List<IUpgradeProgressData> data = new List<IUpgradeProgressData>();

			foreach (var item in itemsList)
			{
				data.Add(new ProgressUpgradeData(item.IdName, item.PointLevel));
				item.SetUpgradeLevel(0);
			}

			return data;
		}
	}
}