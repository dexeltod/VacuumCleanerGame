using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces.Upgrade;
using UnityEngine;

namespace Sources.InfrastructureInterfaces.Configs
{
	public interface IUpgradeEntityViewConfig : IProgressItemConfig
	{
		Sprite Icon { get; }
		IUpgradeElementPrefabView PrefabView { get; }
	}
}