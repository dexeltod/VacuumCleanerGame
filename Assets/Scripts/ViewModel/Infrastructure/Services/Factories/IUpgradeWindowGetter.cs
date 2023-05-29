using View.SceneEntity;

namespace ViewModel.Infrastructure.Services.Factories
{
	public interface IUpgradeWindowGetter : IService
	{
		UpgradeWindow UpgradeWindow { get; }
	}
}