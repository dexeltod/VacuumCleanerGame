using System;
using Sources.DomainInterfaces;

namespace Sources.Domain.Stats
{
	public class PlayerStat : IPlayerStatChangeable
	{
		public string Name { get; private set; }
		public int Value { get; private set; }

		public event Action ValueChanged;

		public PlayerStat(string name, int value)
		{
			Name = name;
			Value = value;
		}

		public void SetValue(int value)
		{
			if (value < 0)
				throw new InvalidCastException("Value must be positive");

			Value = value;
			ValueChanged?.Invoke();
		}
	}
}