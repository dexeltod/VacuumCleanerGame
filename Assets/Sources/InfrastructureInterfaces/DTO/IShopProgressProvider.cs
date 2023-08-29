using Sources.DIService;

namespace Sources.InfrastructureInterfaces.DTO
{
	public interface IShopProgressProvider : IService
	{
		void AddProgressPoint(string progressName);
	}
}