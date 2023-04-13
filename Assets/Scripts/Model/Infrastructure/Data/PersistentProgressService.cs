using Model.Infrastructure.Services;

namespace Model.Infrastructure.Data
{
	public class PersistentProgressService : IPersistentProgressService
	{
		public GameProgress GameProgress { get; set; }
	}
}