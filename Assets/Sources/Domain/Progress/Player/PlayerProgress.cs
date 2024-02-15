using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sources.Domain.Progress.Player
{
	[Serializable] public class PlayerProgress : Progress
	{
		[JsonConstructor]
		public PlayerProgress(List<ProgressUpgradeData> progress, int maxUpgradePointsCount = 0) : base(
			progress,
			maxUpgradePointsCount
		) { }
	}
}