using Sources.Core;
using Sources.View.Interfaces;

namespace Sources.View.Services.UI
{
	public interface IUpgradeWindowGetter : IService
	{
		IUpgradeWindow UpgradeWindow { get; }
	}
}