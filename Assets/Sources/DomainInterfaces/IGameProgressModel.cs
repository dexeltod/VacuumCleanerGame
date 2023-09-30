namespace Sources.DomainInterfaces
{
	public interface IGameProgressModel
	{
		IGameProgress   ShopProgress   { get; }
		IGameProgress   PlayerProgress { get; }
		IResourcesModel ResourcesModel { get; }
	}
}