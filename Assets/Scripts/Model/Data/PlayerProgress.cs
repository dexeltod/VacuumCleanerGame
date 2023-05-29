using System;
using Model.Configs;

namespace Model.Infrastructure.Data
{
	[Serializable]
	public class PlayerProgress
	{
		public int Speed { get; private set; } = 4;
		public int VacuumDistance { get; private set; } = 3;

		public void SetSpeed(int newSpeed) => Speed = newSpeed;

		public void SetVacuumDistance(int newValue) => VacuumDistance = newValue;

		public int MaxFilledScore => MaxFillModifier + GameConfig.DefaultMaxFillCount;
		public int MaxFillModifier { get; private set; } = 0;
		public int CurrentSandCount { get; private set; } = 0;
		public int Money { get; private set; } = 100;

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