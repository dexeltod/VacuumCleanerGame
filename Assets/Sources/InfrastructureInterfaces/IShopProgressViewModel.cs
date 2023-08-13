using Sources.DIService;

namespace Sources.InfrastructureInterfaces
{
	public interface IShopProgressViewModel : IService
	{
		void AddProgressPoint(string progressName);
	}
}