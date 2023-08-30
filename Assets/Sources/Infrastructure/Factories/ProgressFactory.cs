using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Sources.Application.Utils;
using Sources.DIService;
using Sources.Domain.Progress;
using Sources.Domain.Progress.Player;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Upgrade;
using Sources.ServicesInterfaces;

namespace Sources.Infrastructure.Factories
{
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
			IGameProgressModel loadedProgress = await _saveLoadService.LoadFromUnityCloud();
			Init(loadedProgress);
		}

		private void Init(IGameProgressModel loadedProgress)
		{
			if (loadedProgress == null)
			{
				IGameProgressModel newProgress = CreateNewProgress();
				loadedProgress = newProgress;
			}

			_persistentProgressService.Construct(loadedProgress);
		}

		private GameProgressModel CreateNewProgress()
		{
			IUpgradeItemData[] itemsList = _shopFactory.LoadItems();

			GameProgressModel newProgress = CreateProgress(itemsList);

			_persistentProgressService.Construct(newProgress);
			_saveLoadService.SaveToUnityCloud();

			return newProgress;
		}

		private GameProgressModel CreateProgress(IUpgradeItemData[] itemsList)
		{
			IResource<int> soft = GetResource(ResourceType.Soft);
			IResource<int> hard = GetResource(ResourceType.Hard);

			ResourcesData resourcesData = new ResourcesData
			(
				soft,
				hard,
				StartMoneyCount
			);

			PlayerProgress playerProgressModel =
				new PlayerProgress(CreateNewUpgradeProgressData(itemsList));

			ShopProgress shopProgressModel = new(CreateNewUpgradeProgressData(itemsList));

			GameProgressModel newProgress = new GameProgressModel
			(
				resourcesData,
				playerProgressModel,
				shopProgressModel
			);

			return newProgress;
		}

		private IResource<int> GetResource(ResourceType type) =>
			_resourceService.GetResource<int>(type);

		private List<IUpgradeProgressData> CreateNewUpgradeProgressData(IUpgradeItemData[] itemsList)
		{
			List<IUpgradeProgressData> progressList = new List<IUpgradeProgressData>();

			foreach (var itemData in itemsList)
			{
				progressList.Add(new ProgressUpgradeData(itemData.IdName, 0));
				itemData.SetUpgradeLevel(0);
			}

			return progressList;
		}

		// [Serializable]
		// public class HardCurrency
		// {
		// 	[SerializeField] public int Count;
		// 	[SerializeField] public int ResourceType;
		// }
		//
		// [Serializable]
		// public class PlayerProgress
		// {
		// 	[SerializeField] public int MaxPointCount;
		// }
		//
		// [Serializable]
		// public class ResourcesData
		// {
		// 	[SerializeField] public int CurrentSandCount;
		// 	[SerializeField] public HardCurrency HardCurrency;
		// 	[SerializeField] public int MaxFillModifier;
		// 	[SerializeField] public int MaxFilledScore;
		// 	[SerializeField] public SoftCurrency SoftCurrency;
		// }
		//
		// [Serializable]
		// public class Root
		// {
		// 	[SerializeField] public PlayerProgress PlayerProgress;
		// 	[SerializeField] public ResourcesData ResourcesData;
		// 	[SerializeField] public ShopProgress ShopProgress;
		// }
		//
		// [Serializable]
		// public class ShopProgress
		// {
		// 	[SerializeField] public int MaxPointCount;
		// }
		//
		// [Serializable]
		// public class SoftCurrency
		// {
		// 	[SerializeField] public int Count;
		// 	[SerializeField] public int ResourceType;
		// }
	}
}