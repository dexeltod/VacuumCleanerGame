using System.Collections.Generic;
using UnityEngine;

namespace Sources.Infrastructure.Factories.UpgradeEntitiesConfigs
{
	[CreateAssetMenu(fileName = "StartStats", menuName = "Data/Shop/Upgrade/StartStats")]
	public class StartStatsConfig : ScriptableObject
	{
		[SerializeField] private StartStatConfig[] _stat;

		public IReadOnlyCollection<StartStatConfig> Stats => _stat;
	}
}