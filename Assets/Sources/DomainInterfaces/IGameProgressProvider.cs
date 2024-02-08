namespace Sources.DomainInterfaces
{
	public interface IGameProgressProvider
	{
		IGameProgress ShopProgress { get; }
		IGameProgress LevelProgress { get; }
		IGameProgress PlayerProgress { get; }
		IResourcesModel ResourcesModel { get; }
	}
}