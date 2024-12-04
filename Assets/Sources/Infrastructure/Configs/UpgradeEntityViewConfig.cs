using System;
using Sources.InfrastructureInterfaces.Configs;
using Sources.Presentation.UI.Shop;
using Sources.PresentationInterfaces;
using UnityEngine;

namespace Sources.Infrastructure.Configs
{
	[Serializable] public class UpgradeEntityViewConfig : ProgressItemConfig, IUpgradeEntityViewConfig
	{
		[SerializeField] private UpgradeElementPrefabView _upgradeElementPrefabView;

		[SerializeField] private Sprite _icon;

		public Sprite Icon => _icon;
		public IUpgradeElementPrefabView PrefabView => _upgradeElementPrefabView;
	}
}