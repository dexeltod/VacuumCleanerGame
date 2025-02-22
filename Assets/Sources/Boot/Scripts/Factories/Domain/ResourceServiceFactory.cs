using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Boot.Scripts.UpgradeEntitiesConfigs;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Domain.Progress.Entities.Values;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Configs;
using Sources.Utils;
using Sources.Utils.Enums;

namespace Sources.Boot.Scripts.Factories.Domain
{
	public class ResourceServiceFactory
	{
		private const int MaxValue = int.MaxValue - 1000;

		private readonly IReadOnlyCollection<PlayerUpgradeShopViewConfig> _config;
		private readonly IAssetLoader _loader;

		private Dictionary<int, IResource<int>> _resources = new();

		public ResourceServiceFactory(UpgradesListConfig config)
		{
			if (config == null) throw new ArgumentNullException(nameof(config));

			_config = config.ReadOnlyItems;
		}

		public Dictionary<int, IResource<int>> CreateIntCurrencies()
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
				MaxValue
			);

			_resources.Add(globalScore.Id, globalScore);
		}

		private void AddResourceByEnum(Enum value)
		{
			var entity = new IntEntity(
				StaticIdRepository.GetOrAddByEnum(value),
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
