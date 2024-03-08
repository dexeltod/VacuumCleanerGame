using System;
using Sources.Domain.Progress;
using Sources.Infrastructure.Common.Factory;

namespace Sources.Infrastructure.Factories.Domain
{
	public class LevelProgressFactory : Factory<LevelProgress>
	{
		private readonly int _firstLevel;
		private readonly int _maxPoint;

		public LevelProgressFactory(int firstLevel, int maxPoint)
		{
			if (maxPoint < 0) throw new ArgumentOutOfRangeException(nameof(maxPoint));
			if (firstLevel < 0) throw new ArgumentOutOfRangeException(nameof(firstLevel));
			
			_firstLevel = firstLevel;
			_maxPoint = maxPoint;
		}

		public override LevelProgress Create() =>
			new(_firstLevel, _maxPoint);
	}
}