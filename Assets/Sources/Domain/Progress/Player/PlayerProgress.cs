using System;
using Sources.Domain.Temp;

namespace Sources.Domain.Progress.Player
{
	[Serializable] public class PlayerProgress : ProgressEntity
	{
		public PlayerProgress(int currentLevel, int configName) : base(currentLevel, configName) { }
	}
}