using System;
using Sources.Domain.Progress.ResourcesData;

namespace Sources.Domain.Progress.Entities.Values
{
	[Serializable]
	public class IntEntityValue : Resource<int>
	{
		public IntEntityValue(int id, string name, int count, int maxValue) : base(id, name, count, maxValue)
		{
		}

		public override int Value
		{
			get => _value;
			set => Set(value);
		}

		public override int MaxValue
		{
			get => _maxValue;
			set => SetMax(value);
		}

		private int Set(int value)
		{
			if (value < 0 || value > ReadOnlyMaxValue + 1) throw new ArgumentOutOfRangeException(nameof(value));

			_value = value;
			Changed?.Invoke();

			Math.Clamp(_value, 0, MaxValue);

			if (_value > ReadOnlyMaxValue / 2)
				HalfReached?.Invoke();

			return value;
		}

		private void SetMax(int value)
		{
			if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));

			Changed?.Invoke();
			_maxValue = value;
		}

		public override event Action Changed;
		public override event Action HalfReached;
	}
}