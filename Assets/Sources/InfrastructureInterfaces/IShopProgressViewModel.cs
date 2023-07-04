namespace InfrastructureInterfaces
{
	public interface IShopProgressViewModel : IService
	{
		void AddProgressPoint(string progressName);
	}
}