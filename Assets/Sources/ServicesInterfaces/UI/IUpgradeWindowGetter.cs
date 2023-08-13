using Sources.DIService;

namespace Sources.ServicesInterfaces.UI
{
	public interface IUpgradeWindowGetter : IService
	{
		IUpgradeWindow UpgradeWindow { get; }
	}
}