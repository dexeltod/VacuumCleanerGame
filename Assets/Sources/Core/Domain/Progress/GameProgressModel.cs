using System;
using Sources.Core.Domain.Progress.Player;

namespace Sources.Core.Domain.Progress
{
	[Serializable]
	public class GameProgressModel
	{
		public readonly Player.ResourcesData ResourcesData;
		public readonly PlayerProgress PlayerProgress;
		public readonly ShopProgress ShopProgress;

		public GameProgressModel(Player.ResourcesData resourcesData, PlayerProgress playerProgress, ShopProgress shopProgress)
		{
			ResourcesData = resourcesData;
			PlayerProgress = playerProgress;
			ShopProgress = shopProgress;
		}
	}
}