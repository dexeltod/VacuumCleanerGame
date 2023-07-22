using Sources.Core;

namespace Sources.Infrastructure.InfrastructureInterfaces
{
	public interface IShopProgressViewModel : IService
	{
		void AddProgressPoint(string progressName);
	}
}