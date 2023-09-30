using Sources.DIService;

namespace Sources.InfrastructureInterfaces.DTO
{
	public interface IPlayerProgressProvider : IService
	{
		void SetProgress(string progressName);
	}
}