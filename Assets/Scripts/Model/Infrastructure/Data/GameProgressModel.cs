using System;

namespace Model.Infrastructure.Data
{
	[Serializable]
	public class GameProgressModel
	{
		public readonly PlayerProgress PlayerProgress;

		public GameProgressModel()
		{
			PlayerProgress = new PlayerProgress();
		}

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

	[Serializable]
	public class PlayerProgress
	{
		public int Speed { get; private set; } = 3;
		public int VacuumDistance { get; private set; } = 3;
		public void SetSpeed(int newSpeed) =>
			Speed = newSpeed;

		public void SetVacuumDistance(int newValue) => 
			VacuumDistance = newValue;
	}
}