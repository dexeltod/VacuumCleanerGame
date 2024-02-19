using System;
using Sources.DomainInterfaces;
using UnityEngine;

namespace Sources.Domain.Progress
{
	[Serializable] public class LevelProgress : ILevelProgress
	{
		[SerializeField] private int _currentLevel;

		public int CurrentLevel => _currentLevel;

		public LevelProgress(int firstLevel) =>
			_currentLevel = firstLevel;

		public void AddLevel(int level) =>
			_currentLevel += level;
	}
}