namespace ViewModel.Infrastructure.Services.DataViewModel
{
	public interface IShopProgressViewModel : IService
	{
		void AddProgressPoint(string progressName);
	}
}