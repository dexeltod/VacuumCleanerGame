using System;
using Sources.Domain.Progress.Entities.Values;
using Sources.DomainInterfaces.Entities;
using UnityEngine;

namespace Sources.Domain.Stats
{
	[Serializable] public sealed class Stat : IStat
	{
		[SerializeField] private float _startValue;
		[SerializeField] private IntEntityValue _currentLevel;

		[SerializeField] private float _value;
		[SerializeField] private int _id;

		public Stat(float startValue, IntEntityValue currentLevel, int id)
		{
			if (startValue < 0) throw new ArgumentOutOfRangeException(nameof(startValue));

			_startValue = startValue;
			_currentLevel = currentLevel ?? throw new ArgumentNullException(nameof(currentLevel));
			_value = startValue;
			_id = id;
		}

		public float Value => _value;
		public int Id => _id;
		public event Action Changed;

		public void Enable() =>
			_currentLevel.Changed += OnLevelChanged;

		public void Disable() =>
			_currentLevel.Changed -= OnLevelChanged;

		public void Increase(float value)
		{
			if (value <= 0)
				throw new ArgumentOutOfRangeException($"to increase stat, value must be > 0", nameof(value));

			_value += value;
			Changed?.Invoke();
		}

		public void Decrease(float value)
		{
			if (value <= 0)
				throw new ArgumentOutOfRangeException($"to increase stat, value must be > 0", nameof(value));

			_value -= value;

			Math.Clamp(_value, 0, int.MaxValue);
			Changed?.Invoke();
		}

		public void Clear() =>
			_value = _startValue;

		public void Set(float value)
		{
			if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
			_value = value;
			_startValue = value;
			Changed?.Invoke();
		}

		private void OnLevelChanged()
		{
			_startValue = _currentLevel.Value;
			Changed?.Invoke();
		}
	}
}