using System;
using UnityEngine;

namespace Sources.InfrastructureInterfaces.Configs
{
	[Serializable]
	public class PlayerUpgradeShopViewsConfig : ProgressItemConfig, IUpgradeEntityViewConfig
	{
		[SerializeField] private GameObject _upgradeElementPrefabView;

		[SerializeField] private Sprite _icon;

		public Sprite Icon => _icon;
		public GameObject PrefabView => _upgradeElementPrefabView;
	}
}