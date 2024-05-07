using Sources.DomainInterfaces.Models;
using Sources.DomainInterfaces.Models.Shop;

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