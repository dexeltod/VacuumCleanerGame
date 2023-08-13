
using System;
using Sources.Domain.Progress.Player;
using Sources.DomainInterfaces;

namespace Sources.Domain.Progress
{
	[Serializable]
	public class GameProgressModel : IGameProgressModel
	{
		public IGameProgress ShopProgress { get; private set; }
		public IGameProgress PlayerProgress { get; private set;}
		public IResourcesData ResourcesData { get; private set;}

		public GameProgressModel(Player.ResourcesData resourcesData, PlayerProgress playerProgress, ShopProgress shopProgress)
		{
			ResourcesData = resourcesData;
			PlayerProgress = playerProgress;
			ShopProgress = shopProgress;
		}

	}
}