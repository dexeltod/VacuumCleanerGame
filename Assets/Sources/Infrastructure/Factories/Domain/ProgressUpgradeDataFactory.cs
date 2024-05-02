using System;
using System.Collections.Generic;
using Sources.Domain.Progress;
using Sources.Infrastructure.Common.Factory;
using Sources.ServicesInterfaces.Upgrade;

namespace Sources.Infrastructure.Factories.Domain
{
	// public class ProgressUpgradeDataFactory : Factory<List<ProgressUpgradeData>>
	// {
	// 	private readonly IEnumerable<IProgressItemConfig> _itemsList;
	//
	// 	public ProgressUpgradeDataFactory(List<IProgressItemConfig> itemsList) =>
	// 		_itemsList = itemsList ?? throw new ArgumentNullException(nameof(itemsList));
	//
	// 	public override List<UpgradeEntity> LoadList()
	// 	{
	// 		List<UpgradeEntity> progressList = new List<UpgradeEntity>();
	//
	// 		foreach (IProgressItemConfig itemData in _itemsList)
	// 		{
	// 			progressList.Add(new UpgradeEntity(itemData.IdName, 0, itemData.MaxProgressCount));
	// 			itemData.SetUpgradeLevel(0);
	// 		}
	//
	// 		return progressList;
	// 	}
	// }
}