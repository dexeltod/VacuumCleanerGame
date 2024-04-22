using Sources.ControllersInterfaces.Common;
using Sources.Utils;

namespace Sources.ControllersInterfaces
{
	public interface IUpgradeElementPresenter : IPresenter
	{
		void Upgrade(int id);
	}
}