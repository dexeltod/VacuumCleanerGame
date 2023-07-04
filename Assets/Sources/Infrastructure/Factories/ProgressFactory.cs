using System.Collections.Generic;
using Application.UpgradeShop;
using Cysharp.Threading.Tasks;
using Domain.Progress;
using Domain.Progress.Player;
using Domain.Progress.ResourcesData;
using InfrastructureInterfaces;

namespace Infrastructure.Factories
{
	public class ProgressFactory
	{
		private const int StartMoneyCount = 1000;

		private readonly ISaveLoadDataService _saveLoadService;
		private readonly IPersistentProgressService _persistentProgressService;
		private readonly ShopItemFactory _shopFactory;

		public ProgressFactory(ISaveLoadDataService saveLoadService,
			IPersistentProgressService persistentProgressService)
		{
			_saveLoadService = saveLoadService;
			_persistentProgressService = persistentProgressService;
			_shopFactory = new ShopItemFactory();
		}

		public async UniTask UpdateProgress()
		{
			GameProgressModel loadedProgress = _saveLoadService.LoadProgress();
			await Init(loadedProgress);
		}

		private async UniTask Init(GameProgressModel loadedProgress)
		{
			if (loadedProgress == null)
			{
				var newProgress = await CreateNewProgress();
				loadedProgress = newProgress;
			}

			_persistentProgressService.Construct(loadedProgress);
		}

		private async UniTask<GameProgressModel> CreateNewProgress()
		{
			UpgradeItemList itemsList = await _shopFactory.LoadItems();

			List<string> itemsNames = LoadNewProgress(itemsList);

			GameProgressModel newProgress = CreateProgress(itemsList, itemsNames);

			_persistentProgressService.Construct(newProgress);
			_saveLoadService.SaveProgress();

			return newProgress;
		}

		private List<int> GetZeroPoints(UpgradeItemList itemsList)
		{
			List<int> points = new List<int>(itemsList.Items.Count);

			for (int i = 0; i < points.Capacity; i++)
				points.Add(0);

			return points;
		}

		private List<string> LoadNewProgress(UpgradeItemList itemsList)
		{
			List<string> progressNames = new List<string>();

			foreach (var item in itemsList.Items)
				progressNames.Add(item.GetProgressName());

			return progressNames;
		}

		private GameProgressModel CreateProgress(UpgradeItemList itemsList, List<string> itemsNames)
		{
			ResourcesData resourcesData = new ResourcesData(new Resource(ResourceType.Soft), StartMoneyCount);

			PlayerProgress playerProgressModel = new PlayerProgress(GetZeroPoints(itemsList), itemsNames);
			ShopProgress shopProgressModel = new(GetZeroPoints(itemsList), itemsNames, new List<int>());

			GameProgressModel newProgress = new GameProgressModel
			(
				resourcesData,
				playerProgressModel,
				shopProgressModel
			);

			return newProgress;
		}
	}
}