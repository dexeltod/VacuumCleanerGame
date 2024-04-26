using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Domain.Common;
using Sources.Domain.Player;
using Sources.Domain.Progress.Entities.Values;
using Sources.Domain.Temp;
using Sources.Infrastructure.Configs;
using Sources.Infrastructure.Factories.UpgradeShop;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.Repository;
using Sources.ServicesInterfaces;
using Sources.Utils;

namespace Sources.Infrastructure.Factories.Domain
{
	public class PlayerModelFactory
	{
		private readonly IAssetFactory _assetFactory;
		// private readonly IModifiableStatsRepositoryProvider _modifiableStatsRepositoryProvider;

		public PlayerModelFactory(
			// IModifiableStatsRepositoryProvider modifiableStatsRepositoryProvider,
			IAssetFactory assetFactory
		)
		{
			// _modifiableStatsRepositoryProvider = modifiableStatsRepositoryProvider;
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
		}

		private string ShopItems => ResourcesAssetPath.Configs.ShopItems;

		private IModifiableStatsRepository ModifiableStatsRepository =>
			_modifiableStatsRepositoryProvider.Implementation;

		public PlayerModel Create()
		{
			UpgradeEntityListConfig configs = _assetFactory.LoadFromResources<UpgradeEntityListConfig>
				(ShopItems);

			return new PlayerModel(
				configs
					.ReadOnlyItems
					.Select(
						config => new StatChangeable(0, new IntEntityValue(0), config.Id)
					).ToList()
			);
		}
	}
}