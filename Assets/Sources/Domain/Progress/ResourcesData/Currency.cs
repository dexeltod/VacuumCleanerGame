using System;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Utils;

namespace Sources.Domain.Progress.ResourcesData
{
	[Serializable] public abstract class Currency<T> : Resource<T>, ICurrencyReadOnlyProgressValue<T>
	{
		private CurrencyResourceType _resourceType;

		protected Currency(CurrencyResourceType currencyResourceType, T value) : base(value) =>
			_resourceType = currencyResourceType;

		public CurrencyResourceType ResourceType => _resourceType;
	}
}