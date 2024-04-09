using Sources.ControllersInterfaces.Common;

namespace Sources.ControllersInterfaces
{
	public interface IUpgradeWindowPresenter : IPresenter
	{
		void SetMoney(int money);
		void EnableWindow();
	}
}