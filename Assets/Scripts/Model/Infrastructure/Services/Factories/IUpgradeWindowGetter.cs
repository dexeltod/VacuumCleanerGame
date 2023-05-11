using Presenter.SceneEntity;

namespace Model.Infrastructure.Services.Factories
{
	public interface IUpgradeWindowGetter : IService
	{
		UpgradeWindow UpgradeWindow { get; }
	}
}