using Sources.ControllersInterfaces;

namespace Sources.Infrastructure.Services.SceneTriggers
{
	public interface ITriggerReload
	{
		IResourcesProgressPresenter ResourceProgress { get; }
		int CurrentScore { get; }
	}
}
