using UnityEngine;

namespace Sources.ServicesInterfaces.UI
{
	public interface IUpgradeWindowFactory : IUpgradeWindowGetter
	{
		IUpgradeWindow Create();
	}
}