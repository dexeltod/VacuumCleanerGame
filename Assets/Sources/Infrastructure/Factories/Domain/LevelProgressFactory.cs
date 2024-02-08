using System;
using System.Collections.Generic;
using Sources.Domain.Progress;
using Sources.Infrastructure.Common.Factory;
using Sources.Utils.ConstantNames;

namespace Sources.Infrastructure.Factories.Domain
{
	public class LevelProgressFactory : Factory<LevelProgress>
	{
		private const int FirstLevelIndex = 1;

		private readonly ProgressConstantNames _progressConstantNames;

		public LevelProgressFactory(ProgressConstantNames progressConstantNames) =>
			_progressConstantNames
				= progressConstantNames ?? throw new ArgumentNullException(nameof(progressConstantNames));

		public override LevelProgress Create() =>
			new(
				new List<ProgressUpgradeData>
				{
					new(_progressConstantNames.CurrentLevel, FirstLevelIndex)
				}
			);
	}
}