using System;
using Sources.BusinessLogic.Configs;
using UnityEngine;

namespace Sources.Infrastructure.Configs
{
	[Serializable]
	public class PlayerUpgradeShopViewConfig : ProgressItemConfig, IUpgradeEntityViewConfig
	{
		[SerializeField] private GameObject _upgradeElementPrefabView;

		[SerializeField] private Sprite _icon;
		[SerializeField] private AudioSource _sound;
		public AudioSource Sound => _sound;

		public Sprite Icon => _icon;
		public GameObject PrefabView => _upgradeElementPrefabView;
	}
}
