using System;
using System.Collections.Generic;
using System.Linq;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Utils;

namespace Sources.Services.DomainServices
{
	public class ResourcesService : IResourceService
	{
		private readonly Dictionary<ResourceType, IResource<int>> _intResources;
		private readonly Dictionary<ResourceType, IResource<float>> _floatResources;

		public ResourcesService(
			Dictionary<ResourceType, IResource<int>> intResources,
			Dictionary<ResourceType, IResource<float>> floatResources
		)
		{
			_intResources = intResources;
			_floatResources = floatResources;
		}

		public IResource<T> GetResource<T>(ResourceType type) =>
			FindResource<T>(type);

		public void Set<T>(ResourceType type, T value)
		{
			IResource<T> resource = FindResource<T>(type);

			if (value.Equals(resource) == false)
				throw new ArgumentException($"Resource count is not {typeof(T)}");

			resource.Set(value);
		}

		private IResource<T> FindResource<T>(ResourceType resourceType)
		{
			SeeContaining(resourceType);

			if (typeof(T) == typeof(float))
				return (IResource<T>)_floatResources.FirstOrDefault(element => element.Key == resourceType).Value;
			if (typeof(T) == typeof(int))
				return (IResource<T>)_intResources.FirstOrDefault(element => element.Key == resourceType).Value;

			throw new InvalidOperationException("Unknown resource type");
		}

		private void SeeContaining(ResourceType type)
		{
			if (_floatResources.ContainsKey(type) != false) return;

			if (_intResources.ContainsKey(type) == false)
				throw new ArgumentException($"Resource {type} does not exist in current context");
		}
	}
}