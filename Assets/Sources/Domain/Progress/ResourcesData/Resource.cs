using System;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using UnityEngine;

namespace Sources.Domain.Progress.ResourcesData
{
	[Serializable] public abstract class Resource<T> : IResource<T>
	{
		[SerializeField] private T _value;

		protected Resource(T value) =>
			_value = value;

		public T Value => _value;
		public event Action Changed;

		public void Set(T value)
		{
			_value = value ?? throw new ArgumentNullException(nameof(value));
			Changed?.Invoke();
		}
	}
}