using Sources.DomainInterfaces.Entities;
using Sources.DomainInterfaces.Models;
using Sources.DomainInterfaces.Models.Shop;

namespace Sources.DomainInterfaces
{
	public interface IGlobalProgress
	{
		ILevelProgress LevelProgress { get; }
		IResourceModel ResourceModel { get; }
		IShopModel ShopModel { get; }
		IPlayerStatsModel PlayerStatsModel { get; }
		ISoundSettings SoundSettings { get; }
		bool Validate();
	}
}
