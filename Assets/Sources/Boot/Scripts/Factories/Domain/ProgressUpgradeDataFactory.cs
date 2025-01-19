namespace Sources.Boot.Scripts.Factories.Domain
{
	// public class ProgressUpgradeDataFactory : Factory<List<ProgressUpgradeData>>
	// {
	// 	private readonly IEnumerable<IProgressItemConfig> _itemsList;
	//
	// 	public ProgressUpgradeDataFactory(List<IProgressItemConfig> itemsList) =>
	// 		_itemsList = itemsList ?? throw new ArgumentNullException(nameof(itemsList));
	//
	// 	public override List<StatUpgradeEntity> LoadList()
	// 	{
	// 		List<StatUpgradeEntity> progressList = new List<StatUpgradeEntity>();
	//
	// 		foreach (IProgressItemConfig itemData in _itemsList)
	// 		{
	// 			progressList.Add(new StatUpgradeEntity(itemData.IdName, 0, itemData.MaxProgressCount));
	// 			itemData.SetUpgradeLevel(0);
	// 		}
	//
	// 		return progressList;
	// 	}
	// }
}