using UnityEngine;

namespace Sources.BusinessLogic.Configs
{
	public interface IUpgradeEntityViewConfig : IProgressItemConfig
	{
		Sprite Icon { get; }
		GameObject PrefabView { get; }
		AudioSource Sound { get; }
	}
}
