using System;
using System.Collections.Generic;
using Sources.Domain.Progress;
using Sources.Infrastructure.Common.Factory;
using Sources.ServicesInterfaces.Upgrade;

namespace Sources.Infrastructure.Factories.Domain
{
	public class ProgressUpgradeDataFactory : Factory<List<ProgressUpgradeData>>
	{
		private readonly IEnumerable<IUpgradeItemData> _itemsList;

		public ProgressUpgradeDataFactory(IEnumerable<IUpgradeItemData> itemsList) =>
			_itemsList = itemsList ?? throw new ArgumentNullException(nameof(itemsList));

		public override List<ProgressUpgradeData> Create()
		{
			List<ProgressUpgradeData> progressList = new List<ProgressUpgradeData>();

			foreach (IUpgradeItemData itemData in _itemsList)
			{
				progressList.Add(new ProgressUpgradeData(itemData.IdName, 0));
				itemData.SetUpgradeLevel(0);
			}

			return progressList;
		}
	}
}