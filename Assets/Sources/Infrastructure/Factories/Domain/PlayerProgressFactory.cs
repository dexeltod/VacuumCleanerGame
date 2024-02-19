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
		private readonly int _maxUpgradePointsCount;

		public PlayerProgressFactory(List<ProgressUpgradeData> itemsList, int maxUpgradePointsCount)
		{
			if (maxUpgradePointsCount <= 0) throw new ArgumentOutOfRangeException(nameof(maxUpgradePointsCount));
			_itemsList = itemsList ?? throw new ArgumentNullException(nameof(itemsList));
			_maxUpgradePointsCount = maxUpgradePointsCount;
		}

		public override PlayerProgress Create() =>
			new PlayerProgress(_itemsList, _maxUpgradePointsCount);
	}
}