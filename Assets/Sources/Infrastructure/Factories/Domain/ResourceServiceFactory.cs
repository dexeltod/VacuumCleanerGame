using System;
using System.Collections.Generic;
using Sources.Domain.Progress.ResourcesData;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Utils;

namespace Sources.Infrastructure.Factories.Domain
{
	public class ResourceServiceFactory
	{
		public Dictionary<int, IResource<int>> CreateIntCurrencies()
		{
			string[] names = Enum.GetNames(typeof(CurrencyResourceType));
			Array values = Enum.GetValues(typeof(CurrencyResourceType));

			var dictionary = new Dictionary<int, IResource<int>>();

			for (var i = 0; i < names.Length; i++)
			{
				var id = (int)values.GetValue(i);

				dictionary.Add(id, new IntCurrency(id, names[i], 0));
			}

			return dictionary;
		}
	}
}
