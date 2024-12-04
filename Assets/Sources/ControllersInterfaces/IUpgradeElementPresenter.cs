using Sources.ControllersInterfaces.Common;

namespace Sources.ControllersInterfaces
{
	public interface IUpgradeElementPresenter : IPresenter
	{
		void Upgrade(int id);
	}
}