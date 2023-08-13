namespace Sources.DomainInterfaces
{
	public interface IGameProgressModel
	{
		IGameProgress ShopProgress { get; }
		IGameProgress PlayerProgress { get; }
		IResourcesData ResourcesData { get; }
	}
}