using Sources.Domain.Player;
using Sources.Domain.Progress;
using Sources.Domain.Progress.Player;
using Sources.Domain.Settings;

namespace Sources.Domain
{
	public class GameProgressDataTransferObject
	{
		public ResourceModel ResourceModel { get; set; }
		public LevelProgress LevelProgress { get; set; }
		public ShopModel ShopModel { get; set; }
		public PlayerStatsModel PlayerStatsModel { get; set; }
		public SoundSettings SoundSettings { get; set; }
	}
}
