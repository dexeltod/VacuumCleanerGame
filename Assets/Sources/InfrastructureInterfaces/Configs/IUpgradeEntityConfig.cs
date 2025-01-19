using UnityEngine;

namespace Sources.InfrastructureInterfaces.Configs
{
	public interface IUpgradeEntityConfig : IProgressItemConfig
	{
		Sprite Icon { get; }
		GameObject PrefabView { get; }
	}
}
