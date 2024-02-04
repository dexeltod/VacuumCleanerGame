using System;
using System.Collections.Generic;
using Sources.Domain.Progress;
using Sources.Domain.Progress.Player;
using Sources.Domain.Progress.ResourcesData;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.InfrastructureInterfaces.Factory;
using Sources.ServicesInterfaces.Upgrade;
using Sources.Utils;
using Sources.Utils.ConstantNames;
using VContainer;

namespace Sources.Infrastructure.Factories.Player
{
	public class InitialProgressFactory
	{
		private const int StartCurrencyCount = 99999;
		private const int MaxUpgradePointsCount = 6;
		private const int FirstLevelIndex = 1;
		private const int StartScoreCount = 0;

		private readonly IProgressUpgradeFactory _progressUpgradeFactory;
		private readonly IResourceService _resourceService;
		private readonly ProgressConstantNames _progressConstantNames;

		[Inject]
		public InitialProgressFactory(
			IProgressUpgradeFactory progressUpgradeFactory,
			IResourceService resourceService,
			ProgressConstantNames progressConstantNames
		)
		{
			_progressUpgradeFactory = progressUpgradeFactory ??
				throw new ArgumentNullException(nameof(progressUpgradeFactory));
			_resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));
			_progressConstantNames
				= progressConstantNames ?? throw new ArgumentNullException(nameof(progressConstantNames));
		}

		public IGameProgressModel Create()
		{
			IUpgradeItemData[] itemsList = _progressUpgradeFactory.LoadItems();
			GameProgressModel newProgress = CreateProgress(itemsList);
			return newProgress;
		}

		private GameProgressModel CreateProgress(IUpgradeItemData[] itemsList)
		{
			Resource<int> soft = GetResource(ResourceType.Soft);
			Resource<int> hard = GetResource(ResourceType.Hard);
			Resource<int> cashScore = GetResource(ResourceType.CashScore);
			Resource<int> globalScore = GetResource(ResourceType.GlobalScore);

			ResourcesModel resourcesModel = CreateResourceModel(soft, hard, cashScore, globalScore);

			PlayerProgress playerProgressModel = new PlayerProgress(CreateNewUpgradeProgressData(itemsList));

			UpgradeProgressModel upgradeProgressModelModel
				= new(CreateNewUpgradeProgressData(itemsList), MaxUpgradePointsCount);

			LevelProgress levelProgressModel = new
			(
				new List<ProgressUpgradeData>
				{
					new ProgressUpgradeData(_progressConstantNames.CurrentLevel, FirstLevelIndex)
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

		private ResourcesModel CreateResourceModel(
			Resource<int> soft,
			Resource<int> hard,
			Resource<int> cashScore,
			Resource<int> globalScore
		) =>
			new ResourcesModel(
				soft,
				hard,
				cashScore,
				globalScore,
				StartScoreCount,
				StartCurrencyCount,
				StartCurrencyCount
			);

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