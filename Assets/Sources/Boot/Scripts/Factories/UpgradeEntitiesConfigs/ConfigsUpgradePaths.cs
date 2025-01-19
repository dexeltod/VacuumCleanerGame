using Sources.InfrastructureInterfaces.Configs;
using Sources.Utils.AssetPathAttribute;
using UnityEngine;

namespace Sources.Boot.Scripts.Factories.UpgradeEntitiesConfigs
{
	[CreateAssetMenu(fileName = "ShopItems", menuName = "Data/Shop/Upgrade/Items")]
	public class ConfigsUpgradePaths : ScriptableObject
	{
		[AssetPath.Attribute(typeof(PlayerUpgradeShopConfig))]
		[SerializeField]
		private string _speed;

		[AssetPath.Attribute(typeof(PlayerUpgradeShopConfig))]
		[SerializeField]
		private string _cashScore;

		public string CashScore => _cashScore;
		public string Speed => _speed;
	}
}