using System;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using UnityEngine;

namespace Sources.Domain.Progress.ResourcesData
{
	[Serializable]
	public abstract class Resource<T> : IResource<T>
	{
		[SerializeField] protected T _value;
		[SerializeField] protected int _id;
		[SerializeField] protected T _maxValue;

		protected Resource(int id, string name, T value, T maxValue)
		{
			if (id < 0) throw new ArgumentOutOfRangeException(nameof(id));
			if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));

			Name = name;
			_id = id;
			_value = value;
			_maxValue = maxValue ?? throw new ArgumentNullException(nameof(maxValue));
		}

		public bool IsTotalScoreReached { get; protected set; }

		public int Id => _id;
		public T ReadOnlyValue => Value;

		public string Name { get; }

		public abstract T Value { get; set; }
		public abstract T MaxValue { get; set; }
		public T ReadOnlyMaxValue => MaxValue;
		public abstract event Action Changed;
		public abstract event Action HalfReached;
	}
}
