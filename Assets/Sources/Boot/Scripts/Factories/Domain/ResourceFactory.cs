using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Boot.Scripts.UpgradeEntitiesConfigs;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Domain.Progress.Entities.Values;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Configs;
using Sources.Utils;
using Sources.Utils.Enums;

namespace Sources.Boot.Scripts.Factories.Domain
{
	public class ResourceFactory
	{
		private const int MaxValue = int.MaxValue - 1000;

		private readonly IReadOnlyCollection<PlayerUpgradeShopViewConfig> _config;

		private readonly ILevelProgress _levelProgress;

		private readonly IAssetLoader _loader;

		private Dictionary<int, IResource<int>> _resources = new();

		public ResourceFactory(UpgradesListConfig config, ILevelProgress levelProgress)
		{
			if (config == null) throw new ArgumentNullException(nameof(config));

			_levelProgress = levelProgress ?? throw new ArgumentNullException(nameof(levelProgress));

			_config = config.ReadOnlyItems;
		}

		public Dictionary<int, IResource<int>> CreateIntResource()
		{
			_resources = _config.ToDictionary<PlayerUpgradeShopViewConfig, int, IResource<int>>(
				config => config.Id,
				config => new IntEntity(
					config.Id,
					Enum.GetName(config.Type.GetType(), config.Type),
					config.StartProgress,
					config.MaxProgress
				)
			);

			AddResourceByEnum(ResourceType.Soft);
			AddResourceByEnum(ResourceType.Hard);
			AddCashScore();
			AddGlobalScore();

			return _resources;
		}

		private void AddCashScore()
		{
			PlayerUpgradeShopViewConfig cash = GetCashScoreConfig();
			_resources.Add(
				CreateIdByEnum(ResourceType.CashScore),
				new IntEntity(cash.Id, cash.Title, 0, cash.StartProgress)
			);
		}

		private void AddGlobalScore()
		{
			var globalScore = new IntEntity(
				CreateIdByEnum(ResourceType.GlobalScore),
				Enum.GetName(ResourceType.GlobalScore.GetType(), ResourceType.GlobalScore),
				0,
				_levelProgress.MaxTotalResourceCount
			);

			_resources.Add(globalScore.Id, globalScore);
		}

		private void AddResourceByEnum(Enum value)
		{
			var entity = new IntEntity(
				CreateIdByEnum(value),
				Enum.GetName(value.GetType(), value),
				0,
				MaxValue
			);

			_resources.Add(entity.Id, entity);
		}

		private static int CreateIdByEnum(Enum value) => StaticIdRepository.GetOrAddByEnum(value);

		private PlayerUpgradeShopViewConfig GetCashScoreConfig()
		{
			return _config.FirstOrDefault(elem => elem.Type == ProgressType.MaxCashScore)
			       ?? throw new ArgumentNullException(
				       "_config.FirstOrDefault(elem => elem.Type == (ProgressType)ResourceType.CashScore)"
			       );
		}
	}
}
