using System;
using System.Collections.Generic;

namespace Sources.Core.Domain.Progress.Player
{
	[Serializable]
	public class PlayerProgress : Progress
	{
		public PlayerProgress(List<int> progressValues, List<string> progressNames) : base(progressValues,
			progressNames)
		{
		}
	}
}