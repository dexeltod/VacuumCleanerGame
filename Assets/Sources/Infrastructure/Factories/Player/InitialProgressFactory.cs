using System;
using System.Collections.Generic;
using Sources.Domain.Progress;
using Sources.Domain.Progress.Player;
using Sources.Domain.Progress.ResourcesData;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Upgrade;
using Sources.Utils;
using VContainer;

namespace Sources.Infrastructure.Factories
{
	public class InitialProgressFactory
	{
		private const int StartCount = 99999;
		private const int MaxUpgradePointsCount = 6;

		private readonly IProgressUpgradeFactory _progressUpgradeFactory;
		private readonly IResourceService _resourceService;

		[Inject]
		public InitialProgressFactory(
			IProgressUpgradeFactory progressUpgradeFactory,
			IResourceService resourceService
		)
		{
			_progressUpgradeFactory = progressUpgradeFactory ??
				throw new ArgumentNullException(nameof(progressUpgradeFactory));
			_resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));
		}

		public IGameProgressModel Create()
		{
			IUpgradeItemData[] itemsList = _progressUpgradeFactory.LoadItems();
			GameProgressModel newProgress = CreateProgress(itemsList);
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