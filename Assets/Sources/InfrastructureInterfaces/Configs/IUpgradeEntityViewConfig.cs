using UnityEngine;

namespace Sources.InfrastructureInterfaces.Configs
{
	public interface IUpgradeEntityViewConfig : IProgressItemConfig
	{
		Sprite Icon { get; }
		GameObject PrefabView { get; }
	}
}
