using ViewModel.Infrastructure.Services;

namespace Model.Infrastructure.Data
{
	public class PersistentProgressService : IPersistentProgressService
	{
		public GameProgressModel GameProgress { get; set; }
	}
}