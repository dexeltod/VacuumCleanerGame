using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Sources.Domain.Progress.Player
{
	[Serializable] public class PlayerProgress : Progress
	{
		[SerializeField] private int _points;

		public int Points => _points;

		[JsonConstructor]
		public PlayerProgress
		(
			List<ProgressUpgradeData> progress,
			int                       points
		)
			: base(progress) { }
	}
}