using System;
using System.Collections.Generic;

namespace Model.Data.Player
{
	[Serializable]
	public class PlayerProgress
	{
		private readonly List<int> _progressValues;
		private readonly List<string> _progressNames;
		private bool _isInitialized = false;

		public int Speed { get; private set; } = 4;
		public int VacuumDistance { get; private set; } = 3;

		public PlayerProgress(List<int> progressValues, List<string> progressNames)
		{
			_progressValues = progressValues;
			_progressNames = progressNames;
		}

		public void SetVacuumDistance(int newValue) => VacuumDistance = newValue;
		public void SetSpeed(int newSpeed) => Speed = newSpeed;
	}
}