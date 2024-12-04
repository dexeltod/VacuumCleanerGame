using System;
using System.Collections.Generic;
using System.Linq;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Utils;

namespace Sources.Services.DomainServices
{
	public class ResourcesService : IResourceService
	{
		private readonly Dictionary<CurrencyResourceType, IResource<int>> _intResources;
		private readonly Dictionary<CurrencyResourceType, IResource<float>> _floatResources;

		public ResourcesService(
			Dictionary<CurrencyResourceType, IResource<int>> intResources,
			Dictionary<CurrencyResourceType, IResource<float>> floatResources
		)
		{
			_intResources = intResources;
			_floatResources = floatResources;
		}

		public IResource<T> GetResource<T>(CurrencyResourceType type) =>
			FindResource<T>(type);

		public void Set<T>(CurrencyResourceType type, T value)
		{
			IResource<T> resource = FindResource<T>(type);

			if (value.Equals(resource) == false)
				throw new ArgumentException($"Resource count is not {typeof(T)}");

			resource.Set(value);
		}

		private IResource<T> FindResource<T>(CurrencyResourceType currencyResourceType)
		{
			SeeContaining(currencyResourceType);

			if (typeof(T) == typeof(float))
				return (IResource<T>)_floatResources.FirstOrDefault(element => element.Key == currencyResourceType)
					.Value;
			if (typeof(T) == typeof(int))
				return (IResource<T>)_intResources.FirstOrDefault(element => element.Key == currencyResourceType).Value;

			throw new InvalidOperationException("Unknown resource type");
		}

		private void SeeContaining(CurrencyResourceType type)
		{
			if (_floatResources.ContainsKey(type) != false) return;

			if (_intResources.ContainsKey(type) == false)
				throw new ArgumentException($"Resource {type} does not exist in current context");
		}
	}
}