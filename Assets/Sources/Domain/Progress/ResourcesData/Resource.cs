using System;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Utils;
using UnityEngine;

namespace Sources.Domain.Progress.ResourcesData
{
	[Serializable] public abstract class Resource<T> : IResource<T>
	{
		[SerializeField] private T            _count;
		[SerializeField] private ResourceType _resourceType;

		public T Count => _count;

		public ResourceType    ResourceType => _resourceType;
		public event Action<T> ResourceChanged;

		protected Resource(ResourceType resourceType) =>
			_resourceType = resourceType;

		public void Set(T value)
		{
			_count = value;
			ResourceChanged?.Invoke(_count);
		}
	}
}