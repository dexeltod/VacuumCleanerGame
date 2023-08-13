using System;
using System.Collections.Generic;
using Sources.DomainInterfaces;

namespace Sources.Domain.Progress.Player
{
	[Serializable]
	public class PlayerProgress : Progress
	{
		public PlayerProgress(List<IUpgradeProgressData> progress) : base(progress)
		{
		}
	}
}