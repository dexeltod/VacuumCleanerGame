using System;
using Sources.Domain.Progress;
using Sources.Infrastructure.Common.Factory;

namespace Sources.Infrastructure.Factories.Domain
{
	public class LevelProgressFactory : Factory<LevelProgress>
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

		public override LevelProgress Create() =>
			new(_firstLevel, _maxTotalResourcePoint);
	}
}