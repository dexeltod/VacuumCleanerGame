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
using UnityEngine;

namespace Sources.Infrastructure.Factories
{
	[Serializable]
	public class ProgressFactory : IDisposable
	{
		private const int StartMoneyCount = 99999;

		private readonly IProgressLoadDataService _progressLoadDataService;
		private readonly IPersistentProgressServiceConstructable _persistentProgressService;
		private readonly IShopItemFactory _shopFactory;
		private readonly IProgressClearable _progressClearable;
		private readonly IResourceService _resourceService;

		public ProgressFactory(IProgressLoadDataService progressLoadDataService,
			IPersistentProgressServiceConstructable persistentProgressService, IShopItemFactory shopItemFactory,
			IProgressClearable progressClearable)
		{
			_progressLoadDataService = progressLoadDataService;
			_persistentProgressService = persistentProgressService;
			_shopFactory = shopItemFactory;
			_progressClearable = progressClearable;
			_resourceService = GameServices.Container.Get<IResourceService>();
			_progressClearable.ProgressCleared += CreateNewProgress;
		}

		public void Dispose() =>
			_progressClearable.ProgressCleared -= CreateNewProgress;

		public async UniTask InitializeProgress()
		{
			IGameProgressModel loadedSaves = await _progressLoadDataService.LoadFromCloud();

			Initialize(loadedSaves);
		}

		public async UniTask<IGameProgressModel> Load() =>
			await _progressLoadDataService.LoadFromCloud();

		public void Save(IGameProgressModel model) =>
			_progressLoadDataService.SaveToCloud(model);

		private void Initialize(IGameProgressModel loadedProgress)
		{
			loadedProgress = CreatNewIfNull(loadedProgress);
			_persistentProgressService.Construct(loadedProgress);
		}

		private IGameProgressModel CreatNewIfNull(IGameProgressModel loadedProgress)
		{
			if (loadedProgress == null)
			{
				Debug.Log("Creation new progress model");

				IGameProgressModel newProgress = CreateNewProgress();
				_progressLoadDataService.SaveToCloud(newProgress);
				loadedProgress = newProgress;
			}

			return loadedProgress;
		}

		private GameProgressModel CreateNewProgress()
		{
			IUpgradeItemData[] itemsList = _shopFactory.LoadItems();
			GameProgressModel newProgress = CreateProgress(itemsList);

			_persistentProgressService.Construct(newProgress);

			return newProgress;
		}

		private GameProgressModel CreateProgress(IUpgradeItemData[] itemsList)
		{
			Resource<int> soft = GetResource(ResourceType.Soft);
			Resource<int> hard = GetResource(ResourceType.Hard);
			Resource<int> score = GetResource(ResourceType.Score);

			Debug.Log("Resources loaded");

			ResourcesModel resourcesModel = new ResourcesModel
			(
				soft,
				hard,
				score,
				StartMoneyCount,
				0
			);

			PlayerProgress playerProgressModel = new PlayerProgress(CreateNewUpgradeProgressData(itemsList), 0);

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