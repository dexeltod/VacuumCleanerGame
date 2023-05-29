using System;

namespace Model.Infrastructure.Data
{
	[Serializable]
	public class GameProgressModel
	{
		public readonly PlayerProgress PlayerProgress;
		public readonly ShopProgress ShopProgress;

		public GameProgressModel()
		{
			ShopProgress = new ShopProgress();
			PlayerProgress = new PlayerProgress();
		}
	}
}