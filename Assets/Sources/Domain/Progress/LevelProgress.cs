using System;
using Sources.DomainInterfaces;
using UnityEngine;

namespace Sources.Domain.Progress
{
	[Serializable]
	public class LevelProgress : ILevelProgress
	{
		[SerializeField] private int _currentLevel;
		[SerializeField] private int _maxTotalResourceCount;

		public LevelProgress(int firstLevel, int maxPoint)
		{
			_currentLevel = firstLevel;
			_maxTotalResourceCount = maxPoint;

			Debug.Log(_maxTotalResourceCount);
		}

		public int CurrentLevel => _currentLevel;
		public int MaxTotalResourceCount => _maxTotalResourceCount;

		public void AddLevel(int maxPointDelta, int level)
		{
			_currentLevel += level;
			Debug.Log("LevelProgress: " + _currentLevel);
			_maxTotalResourceCount += maxPointDelta;
		}
	}
}