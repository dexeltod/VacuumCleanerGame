using System;
using Sources.Utils;

namespace Sources.Domain.Progress.ResourcesData
{
	[Serializable] public class IntCurrency : Currency<int>
	{
		public IntCurrency(CurrencyResourceType currencyResourceType, int value) : base(currencyResourceType, value) { }
	}
}