using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
	[Serializable] public class ProgressFactory : IDisposable
	{
		private const int StartCount = 99999;
		private const int MaxUpgradePointsCount = 6;

		private readonly IProgressLoadDataService _progressLoadDataService;
		private readonly IPersistentProgressServiceConstructable _persistentProgressService;
		private readonly IUpgradeDataFactory _shopFactory;
		private readonly IProgressClearable _progressClearable;
		private readonly IResourceService _resourceService;

		public ProgressFactory(
			IProgressLoadDataService progressLoadDataService,
			IPersistentProgressServiceConstructable persistentProgressService,
			IUpgradeDataFactory upgradeDataFactory,
			IProgressClearable progressClearable,
			IResourceService resourceService
		)
		{
			_progressLoadDataService = progressLoadDataService ??
				throw new ArgumentNullException(nameof(progressLoadDataService));
			_persistentProgressService = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));
			_shopFactory = upgradeDataFactory ?? throw new ArgumentNullException(nameof(upgradeDataFactory));
			_progressClearable = progressClearable ?? throw new ArgumentNullException(nameof(progressClearable));
			_resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));

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
			string progressName = "CurrentLevel";

			Resource<int> soft = GetResource(ResourceType.Soft);
			Resource<int> hard = GetResource(ResourceType.Hard);
			Resource<int> cashScore = GetResource(ResourceType.CashScore);
			Resource<int> globalScore = GetResource(ResourceType.GlobalScore);

			ResourcesModel resourcesModel = new ResourcesModel(
				soft,
				hard,
				cashScore,
				globalScore,
				StartCount,
				StartCount
			);

			PlayerProgress playerProgressModel = new PlayerProgress(CreateNewUpgradeProgressData(itemsList));

			UpgradeProgressModel upgradeProgressModelModel
				= new(CreateNewUpgradeProgressData(itemsList), MaxUpgradePointsCount);

			LevelProgress levelProgressModel = new
			(
				new List<ProgressUpgradeData>
				{
					new ProgressUpgradeData(progressName, 1)
				}
			);

			GameProgressModel newProgress = new GameProgressModel(
				resourcesModel,
				playerProgressModel,
				upgradeProgressModelModel,
				levelProgressModel
			);

			return newProgress;
		}

		private List<ProgressUpgradeData> CreateNewUpgradeProgressData(IEnumerable<IUpgradeItemData> itemsList)
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