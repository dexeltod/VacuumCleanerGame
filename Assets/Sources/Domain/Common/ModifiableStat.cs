using System;
using Sources.Domain.Temp;

namespace Sources.Domain.Common
{
	public sealed class ModifiableStat : IModifiableStat
	{
		private readonly int _startValue;

		private int _value;

		public ModifiableStat(int startValue)
		{
			if (startValue <= 0) throw new ArgumentOutOfRangeException(nameof(startValue));
			_startValue = startValue;
			_value = startValue;
		}

		public int Value => _value;

		public void Increase(int value)
		{
			if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
			_value += value;
		}

		public void Decrease(int value)
		{
			if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));

			_value -= value;

			Math.Clamp(_value, 0, int.MaxValue);
		}

		public void Set(int value)
		{
			if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
			_value = value;
		}

		public void Clear() =>
			_value = _startValue;
	}
}