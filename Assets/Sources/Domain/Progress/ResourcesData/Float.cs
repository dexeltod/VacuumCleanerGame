using System;
using Sources.Utils;

namespace Sources.Domain.Progress.ResourcesData
{
	[Serializable] public class Float : Currency<float>
	{
		public Float(CurrencyResourceType currencyResourceType, float value) :
			base(currencyResourceType, value) { }
	}
}