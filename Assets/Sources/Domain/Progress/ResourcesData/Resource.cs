using System;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using UnityEngine;

namespace Sources.Domain.Progress.ResourcesData
{
	[Serializable]
	public abstract class Resource<T> : IResource<T>, IReadOnlyProgress<T>
	{
		[SerializeField] private T _value;
		[SerializeField] private int _id;

		protected Resource(int id, string name, T value)
		{
			if (id < 0) throw new ArgumentOutOfRangeException(nameof(id));
			if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));

			Name = name;
			_id = id;
			_value = value;
		}

		public T Value
		{
			get => _value;
			set
			{
				if (value != null) _value = value;
			}
		}

		public int Id => _id;
		public string Name { get; }
		public event Action Changed;

		public void Set(T value)
		{
			_value = value ?? throw new ArgumentNullException(nameof(value));
			Changed?.Invoke();
		}
	}
}
