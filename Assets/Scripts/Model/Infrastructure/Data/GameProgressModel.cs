using System;

namespace Model.Infrastructure.Data
{
	[Serializable]
	public class GameProgressModel
	{
		public int MaxFilledScore => MaxFillModifier + GameConfig.DefaultMaxFillCount;
		public int MaxFillModifier { get; private set; } = 0;
		public int CurrentSandCount { get; private set; } = 0;
		public int CurrentMoney { get; private set; } = 0;

		public void AddSand(int count) =>
			CurrentSandCount += count;

		public void DecreaseSand(int count) =>
			CurrentSandCount -= count;

		public void AddMoney(int count) =>
			CurrentMoney += count;

		public void DecreaseMoney(int count) =>
			CurrentMoney -= count;
	}
}