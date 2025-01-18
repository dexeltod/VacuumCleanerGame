using System;
using System.Collections.Generic;
using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.Infrastructure.Services.DomainServices
{
	public class ResourcesRepository : IResourcesRepository
	{
		private readonly Dictionary<int, IResource<int>> _intResources;
		private readonly Dictionary<int, IResource<float>> _floatResources;

		public ResourcesRepository(
			Dictionary<int, IResource<int>> intResources
		)
		{
			_intResources = intResources;
		}

		public IResource<T> GetResource<T>(int id) =>
			FindResource<T>(id);

		public void Set<T>(int id, T value)
		{
			IResource<T> resource = FindResource<T>(id);

			if (value.Equals(resource) == false)
				throw new ArgumentException($"Resource count is not {typeof(T)}");

			resource.Set(value);
		}

		private IResource<T> FindResource<T>(int id)
		{
			SeeContaining(id);

			if (typeof(T) == typeof(float))
				return (IResource<T>)_floatResources[id];

			if (typeof(T) == typeof(int))
				return (IResource<T>)_intResources[id];

			throw new InvalidOperationException("Unknown resource type");
		}

		private void SeeContaining(int type)
		{
			if (_intResources.ContainsKey(type) == false)
				throw new ArgumentException($"Resource {type} does not exist in current context");
		}
	}
}