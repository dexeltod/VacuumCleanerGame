using System;
using System.Collections.Generic;

namespace Domain.Progress.Player
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