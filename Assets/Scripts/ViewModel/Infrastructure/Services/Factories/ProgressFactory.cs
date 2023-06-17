using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Model.Data;
using Model.Data.Player;
using Model.UpgradeShop;

namespace ViewModel.Infrastructure.Services.Factories
{
	public class ProgressFactory
	{
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

		public async UniTask LoadProgress()
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
			ResourcesData resourcesData = new ResourcesData(0);
			PlayerProgress playerProgressModel = new PlayerProgress(new List<int>(), new List<string>());

			var shopProgressModel = await CreateNewShopProgress();

			GameProgressModel newProgress =
				new GameProgressModel(resourcesData, playerProgressModel, shopProgressModel);

			_persistentProgressService.Construct(newProgress);
			_saveLoadService.SaveProgress();
			return newProgress;
		}

		private async Task<ShopProgress> CreateNewShopProgress()
		{
			var items = await _shopFactory.LoadItems();
			List<string> itemsNames = new List<string>();

			foreach (var item in items.Items)
				itemsNames.Add(item.GetProgressName());

			List<int> itemsValues = new List<int>();

			foreach (var item in items.Items)
				itemsValues.Add(0);

			ShopProgress shopProgressModel = new(itemsValues, itemsNames);
			return shopProgressModel;
		}
	}
}