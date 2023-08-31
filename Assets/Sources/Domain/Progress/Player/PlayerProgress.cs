using System;
using System.Collections.Generic;

namespace Sources.Domain.Progress.Player
{
	[Serializable]
	public class PlayerProgress : Progress
	{
		public PlayerProgress(List<ProgressUpgradeData> progress) : base(progress)
		{
		}
	}
}