using System;
using System.Collections.Generic;

namespace Sources.Domain.Progress
{
	[Serializable] 
	public class LevelProgress : Progress
	{
		private readonly List<ProgressUpgradeData> _upgradeProgressData;

		public LevelProgress(List<ProgressUpgradeData> progress) : base(progress) =>
			_upgradeProgressData = progress;
	}
}