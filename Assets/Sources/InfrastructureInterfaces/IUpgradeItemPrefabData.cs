using UnityEngine;

namespace Sources.InfrastructureInterfaces
{
	public interface IUpgradeItemPrefabData
	{
		Sprite Icon { get; }
		IUpgradeElementConstructable ElementConstructable { get; }
	}
}