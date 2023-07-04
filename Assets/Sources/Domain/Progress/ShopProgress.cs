using System;
using System.Collections.Generic;

namespace Domain.Progress
{
	[Serializable]
	public class ShopProgress : Progress
	{
		private List<int> _progressPointPrice;
		
		public ShopProgress(List<int> progressPointValues, List<string> progressNames, List<int> prices) : base(progressPointValues,
			progressNames)
		{
			_progressPointPrice = prices;
		}
	}
}