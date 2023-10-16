using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sources.Domain.Progress
{
	[Serializable] public class ShopProgress : Progress
	{
		[JsonConstructor]
		public ShopProgress
		(
			List<ProgressUpgradeData> progress,
			int                       maxUpgradePointsCount
		) : base(progress, maxUpgradePointsCount) { }
	}
}