namespace Sources.DomainInterfaces
{
	public interface IShopProgress : IGameProgress
	{
		int MaxShopPoints { get; }
	}
}