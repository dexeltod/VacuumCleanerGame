namespace Sources.DomainInterfaces
{
	public interface IGameProgressModel
	{
		IGameProgress ShopProgress { get; }
		IGameProgress LevelProgress { get; }
		IGameProgress PlayerProgress { get; }
		IResourcesModel ResourcesModel { get; }
	}
}