using Sources.DomainInterfaces.Models;

namespace Sources.DomainInterfaces
{
	public interface IGlobalProgress
	{
		ILevelProgress LevelProgress { get; }
		IResourceModelReadOnly ResourceModelReadOnly { get; }
		IShopModel ShopModel { get; }
		IPlayerModel PlayerModel { get; }
	}
}