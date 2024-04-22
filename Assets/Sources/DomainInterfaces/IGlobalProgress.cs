namespace Sources.DomainInterfaces
{
	public interface IGlobalProgress
	{
		ILevelProgress LevelProgress { get; }
		IResourceModelReadOnly ResourceModelReadOnly { get; }
		IShopEntity ShopEntity { get; }
	}
}