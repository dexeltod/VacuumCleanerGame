using Sources.Domain.Progress;
using Sources.Infrastructure.Common.Factory;

namespace Sources.Infrastructure.Factories.Domain
{
	public class LevelProgressFactory : Factory<LevelProgress>
	{
		private const int FirstLevelIndex = 1;

	
		public override LevelProgress Create() =>
			new(FirstLevelIndex);
	}
}