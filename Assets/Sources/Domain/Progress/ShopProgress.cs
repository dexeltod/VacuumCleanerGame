using System;
using System.Collections.Generic;
using Sources.DomainInterfaces;

namespace Sources.Domain.Progress
{
	[Serializable]
	public class ShopProgress : Progress
	{
		public ShopProgress(List<IUpgradeProgressData> progress) : base(progress)
		{
		}
	}
}