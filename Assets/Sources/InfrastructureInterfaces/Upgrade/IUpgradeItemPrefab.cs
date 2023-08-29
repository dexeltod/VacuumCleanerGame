using UnityEngine;

namespace Sources.InfrastructureInterfaces.Upgrade
{
	public interface IUpgradeItemPrefab
	{
		Sprite Icon { get; }
		IUpgradeElementConstructable ElementConstructable { get; }
	}
}