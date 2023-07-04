using View.SceneEntity;

namespace InfrastructureInterfaces
{
	public interface IUpgradeWindowGetter : IService
	{
		UpgradeWindow UpgradeWindow { get; }
	}
}