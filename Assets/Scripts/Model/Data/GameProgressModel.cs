using System;
using Model.Data.Player;

namespace Model.Data
{
	[Serializable]
	public class GameProgressModel
	{
		public readonly ResourcesData ResourcesData;

		public readonly PlayerProgress PlayerProgress;

		public readonly ShopProgress ShopProgress;

		public GameProgressModel(ResourcesData resourcesData, PlayerProgress playerProgress, ShopProgress shopProgress)
		{
			ResourcesData = resourcesData;
			PlayerProgress = playerProgress;
			ShopProgress = shopProgress;
		}
	}
}