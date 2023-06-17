namespace ViewModel.Infrastructure.Services.DataViewModel
{
	public interface IPlayerProgressViewModel : IService
	{
		void SetProgress(string progressName);
	}
}