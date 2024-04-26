using System.Collections.Generic;
using Sources.Domain.Progress.ResourcesData;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Utils;

namespace Sources.Infrastructure.Factories.Domain
{
	public class ResourceServiceFactory
	{
		private readonly Dictionary<CurrencyResourceType, IResource<float>> _floatResources = new();
		private readonly Dictionary<CurrencyResourceType, IResource<int>> _intResources = new();

		public ResourceServiceFactory()
		{
			IResource<int> intResourceSoft = new IntCurrency(CurrencyResourceType.Soft, 0);
			IResource<int> intResourceHard = new IntCurrency(CurrencyResourceType.Hard, 0);
			IResource<int> intResourceScore = new IntCurrency(CurrencyResourceType.CashScore, 0);
			IResource<int> intResourceGlobalScore = new IntCurrency(CurrencyResourceType.GlobalScore, 0);

			_intResources.Add(CurrencyResourceType.Soft, intResourceSoft);
			_intResources.Add(CurrencyResourceType.Hard, intResourceHard);
			_intResources.Add(CurrencyResourceType.CashScore, intResourceScore);
			_intResources.Add(CurrencyResourceType.GlobalScore, intResourceGlobalScore);
		}

		public Dictionary<CurrencyResourceType, IResource<int>> GetIntResources() =>
			_intResources;

		public Dictionary<CurrencyResourceType, IResource<float>> GetFloatResources() =>
			_floatResources;
	}
}