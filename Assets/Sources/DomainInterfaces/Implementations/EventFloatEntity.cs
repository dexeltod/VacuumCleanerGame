using System;
using UnityEngine;

namespace Sources.DomainInterfaces.Implementations
{
	[Serializable]
	public class EventFloatEntity
	{
		[SerializeField] private float _value;

		public EventFloatEntity(float value)
		{
			if (value < -80f) throw new ArgumentOutOfRangeException(nameof(value));

			_value = value;
		}

		public float Value
		{
			get => _value;
			set => Set(value);
		}

		private void Set(float value)
		{
			if (value < -80) throw new ArgumentOutOfRangeException(nameof(value));

			_value = value;
			Changed?.Invoke();
		}

		public event Action Changed;
	}
}
