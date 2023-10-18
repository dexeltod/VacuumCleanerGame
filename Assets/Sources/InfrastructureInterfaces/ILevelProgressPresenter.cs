using Sources.DIService;

namespace Sources.InfrastructureInterfaces
{
	public interface ILevelProgressPresenter : IService
	{
		int CurrentLevelNumber { get; }

		void SetNextLevel(int newValue);
	}
}