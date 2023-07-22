using Sources.Core;

namespace Sources.Infrastructure.InfrastructureInterfaces
{
	public interface IPlayerProgressViewModel : IService
	{
		void SetProgress(string progressName);
	}
}