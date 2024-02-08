using System;
using System.Collections.Generic;
using Sources.Domain.Progress.Player;
using Sources.Infrastructure.Common.Factory;
using Sources.ServicesInterfaces.Upgrade;

namespace Sources.Infrastructure.Factories.Domain
{
	public class PlayerProgressFactory : Factory<PlayerProgress>
	{
		private readonly IEnumerable<IUpgradeItemData> _itemsList;

		public PlayerProgressFactory(IEnumerable<IUpgradeItemData> itemsList) =>
			_itemsList = itemsList ?? throw new ArgumentNullException(nameof(itemsList));

		public override PlayerProgress Create() =>
			new PlayerProgress((new ProgressUpgradeDataFactory(_itemsList).Create()));
	}
}