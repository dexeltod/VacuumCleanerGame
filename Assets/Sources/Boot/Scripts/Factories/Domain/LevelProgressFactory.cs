using System;
using Sources.Domain.Progress;

namespace Sources.Boot.Scripts.Factories.Domain
{
	public class LevelProgressFactory
	{
		private readonly int _firstLevel;
		private readonly int _maxTotalResourcePoint;

		public LevelProgressFactory(int firstLevel, int maxTotalResourcePoint)
		{
			if (maxTotalResourcePoint < 0) throw new ArgumentOutOfRangeException(nameof(maxTotalResourcePoint));
			if (firstLevel < 0) throw new ArgumentOutOfRangeException(nameof(firstLevel));

			_firstLevel = firstLevel;
			_maxTotalResourcePoint = maxTotalResourcePoint;
		}

		public LevelProgress Create() => new(_firstLevel, _maxTotalResourcePoint);
	}
}