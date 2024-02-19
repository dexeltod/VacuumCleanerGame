namespace Sources.DomainInterfaces
{
	public interface IGlobalProgress
	{
		IGameProgress ShopProgress { get; }
		ILevelProgress LevelProgress { get; }
		IGameProgress PlayerProgress { get; }
		IResourcesModel ResourcesModel { get; }
	}
}