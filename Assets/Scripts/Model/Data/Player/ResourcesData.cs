using System;
using Model.Configs;

namespace Model.Data.Player
{
	[Serializable]
	public class ResourcesData
	{
		public int Money { get; private set; }
		public int MaxFilledScore => MaxFillModifier + GameConfig.DefaultMaxFillCount;
		public int MaxFillModifier { get; private set; } = 0;
		public int CurrentSandCount { get; private set; } = 0;

		public ResourcesData(int money)
		{
			Money = money;	
		}
		
		public void AddSand(int count) =>
			CurrentSandCount += count;

		public void DecreaseSand(int count) =>
			CurrentSandCount -= count;

		public void AddMoney(int count) =>
			Money += count;

		public void DecreaseMoney(int count) =>
			Money -= count;
	}
}