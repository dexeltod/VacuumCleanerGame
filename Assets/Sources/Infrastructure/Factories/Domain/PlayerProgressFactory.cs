using System;
using System.Collections.Generic;
using Sources.Domain.Progress;
using Sources.Domain.Progress.Player;
using Sources.Infrastructure.Common.Factory;

namespace Sources.Infrastructure.Factories.Domain
{
	public class PlayerProgressFactory : Factory<PlayerProgress>
	{
		private readonly List<ProgressUpgradeData> _itemsList;

		public PlayerProgressFactory(List<ProgressUpgradeData> itemsList)
		{
			_itemsList = itemsList ?? throw new ArgumentNullException(nameof(itemsList));
		}

		public override PlayerProgress Create() =>
			new PlayerProgress(_itemsList);
	}
}