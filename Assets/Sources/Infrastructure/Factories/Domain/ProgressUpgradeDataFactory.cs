using System;
using System.Collections.Generic;
using Sources.Domain.Progress;
using Sources.Domain.Temp;
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
	// 	public override List<ProgressEntity> LoadList()
	// 	{
	// 		List<ProgressEntity> progressList = new List<ProgressEntity>();
	//
	// 		foreach (IProgressItemConfig itemData in _itemsList)
	// 		{
	// 			progressList.Add(new ProgressEntity(itemData.IdName, 0, itemData.MaxProgressCount));
	// 			itemData.SetUpgradeLevel(0);
	// 		}
	//
	// 		return progressList;
	// 	}
	// }
}