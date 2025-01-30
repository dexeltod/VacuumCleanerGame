using System;
using System.Collections.Generic;
using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.Infrastructure.Services.DomainServices
{
	public class ResourcesRepository : IResourcesRepository
	{
		private readonly Dictionary<int, IResource<float>> _floatResources;
		private readonly Dictionary<int, IResource<int>> _intResources;

		public ResourcesRepository(Dictionary<int, IResource<int>> intResources) =>
			_intResources = intResources ?? throw new ArgumentNullException(nameof(intResources));

		public IResource<T> GetResource<T>(int id) => FindResource<T>(id);

		public void Set<T>(int id, T value)
		{
			IResource<T> resource = FindResource<T>(id);

			if (value.Equals(resource) == false)
				throw new ArgumentException($"Resource count is not {typeof(T)}");

			resource.Value = value;
		}

		public void Add<T>(int id, IResource<T> resource) where T : IResource<T>
		{
			if (typeof(T) == typeof(float)) _floatResources?.Add(id, resource as IResource<float>);

			if (typeof(T) == typeof(int)) _intResources?.Add(id, resource as IResource<int>);

			throw new InvalidOperationException("Unknown resource type");
		}

		public void SetMax<T>(int id, T value)
		{
			IResource<T> resource = FindResource<T>(id);

			if (value.Equals(resource) == false)
				throw new ArgumentException($"Resource count is not {typeof(T)}");

			resource.MaxValue = value;
		}

		private IResource<T> FindResource<T>(int id)
		{
			SeeContaining<T>(id);

			if (typeof(T) == typeof(float))
				if (_floatResources != null)
					return (IResource<T>)_floatResources[id];

			if (typeof(T) == typeof(int))
				return (IResource<T>)_intResources[id];

			throw new InvalidOperationException("Unknown resource type");
		}

		private void SeeContaining<T>(int id)
		{
			if (_intResources.ContainsKey(id) == false)
				throw new ArgumentException($"Resource {id} with id {typeof(T)} does not exist in current context");
		}
	}
}
