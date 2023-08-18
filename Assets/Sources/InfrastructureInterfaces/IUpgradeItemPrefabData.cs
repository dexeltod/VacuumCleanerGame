using UnityEngine;

namespace Sources.PresentationInterfaces
{
	public interface IUpgradeItemPrefabData
	{
		Sprite Icon { get; }
		IUpgradeElementConstructable UpgradeElementConstructable { get; }
		
	}
}