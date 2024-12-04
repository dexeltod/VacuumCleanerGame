using Sources.Infrastructure.Configs;
using Sources.Utils.AssetPathAttribute;
using UnityEngine;

namespace Sources.Infrastructure.Factories.UpgradeEntitiesConfigs
{
	[CreateAssetMenu(fileName = "ShopItems", menuName = "Data/Shop/Upgrade/Items")]
	public class ConfigsUpgradePaths : ScriptableObject
	{
		[AssetPath.Attribute(typeof(UpgradeEntityViewConfig))] [SerializeField]
		private string _speed;

		[AssetPath.Attribute(typeof(UpgradeEntityViewConfig))] [SerializeField]
		private string _cashScore;

		public string CashScore => _cashScore;
		public string Speed => _speed;
	}
}