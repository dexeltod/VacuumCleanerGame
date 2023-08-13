using Sources.DIService;

namespace Sources.InfrastructureInterfaces
{
	public interface IPlayerProgressViewModel : IService
	{
		void SetProgress(string progressName);
	}
}