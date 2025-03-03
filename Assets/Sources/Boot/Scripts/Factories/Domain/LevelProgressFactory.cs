using Sources.Domain.Progress;

namespace Sources.Boot.Scripts.Factories.Domain
{
	public class LevelProgressFactory
	{
		private readonly int _firstLevel;
		private readonly int _maxTotalResourcePoint;

		public LevelProgressFactory(int maxTotalResourcePoint, int firstLevel = 1)
		{
			_firstLevel = firstLevel;
			_maxTotalResourcePoint = maxTotalResourcePoint;
		}

		public LevelProgress Create() => new(_firstLevel, _maxTotalResourcePoint);
	}
}