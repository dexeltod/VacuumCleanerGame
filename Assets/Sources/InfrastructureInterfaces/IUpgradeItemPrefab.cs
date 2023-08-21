using UnityEngine;

namespace Sources.InfrastructureInterfaces
{
	public interface IUpgradeItemPrefab
	{
		Sprite Icon { get; }
		IUpgradeElementConstructable ElementConstructable { get; }
	}
}