using System;
using Sources.DomainInterfaces;

namespace Sources.Domain
{
	public class PlayerStat : IPlayerStatChangeable
	{
		public string Name  { get; }
		public int    Value { get; private set; }

		public event Action ValueChanged;

		public PlayerStat(string name, int value)
		{
			Name  = name;
			Value = value;
		}

		public void SetValue(int value)
		{
			if (value < 0)
				throw new InvalidCastException("Value must be positive");

			Value = value;
			ValueChanged.Invoke();

			if (ValueChanged == null)
				throw new NullReferenceException($"{ValueChanged} does not have a subscribers");
		}
	}
}