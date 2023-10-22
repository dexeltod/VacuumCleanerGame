using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sources.Domain.Progress
{
	[Serializable] public class UpgradeProgressModel : Progress
	{
		[JsonConstructor]
		public UpgradeProgressModel
		(
			List<ProgressUpgradeData> progress,
			int                       maxUpgradePointsCount
		) : base(progress, maxUpgradePointsCount) { }
	}
}