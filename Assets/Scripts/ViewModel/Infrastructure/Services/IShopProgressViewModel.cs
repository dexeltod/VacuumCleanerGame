namespace ViewModel.Infrastructure.Services
{
	public interface IShopProgressViewModel : IService
	{
		void AddProgressPoint(string progressName);
	}
}