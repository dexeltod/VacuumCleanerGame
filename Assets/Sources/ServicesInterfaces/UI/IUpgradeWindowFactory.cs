using UnityEngine;

namespace Sources.ServicesInterfaces.UI
{
	public interface IUpgradeWindowFactory : IUpgradeWindowGetter
	{
		GameObject Create();
	}
}