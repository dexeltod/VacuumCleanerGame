using System;
using System.Collections.Generic;
using Sources.DomainInterfaces;

namespace Sources.Domain.Progress
{
	[Serializable]
	public class ShopProgress : Progress
	{
		private List<int> _progressPointPrice;

		public ShopProgress(List<IUpgradeProgressData> progress, List<int> prices) : base(progress)
		{
			_progressPointPrice = prices;
		}
	}
}