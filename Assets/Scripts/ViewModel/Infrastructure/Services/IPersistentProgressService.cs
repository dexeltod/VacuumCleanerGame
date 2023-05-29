using Model.Infrastructure.Data;

namespace ViewModel.Infrastructure.Services
{
	public interface IPersistentProgressService : IService
	{
		GameProgressModel GameProgress { get; set; }
	}
}