using System;
using System.Collections.Generic;

namespace Sources.Domain.Progress
{
	[Serializable]
	public class ShopProgress : Progress
	{
		public ShopProgress(List<ProgressUpgradeData> progress) : base(progress)
		{
		}
	}
}