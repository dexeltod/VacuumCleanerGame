using Model.Infrastructure.Data;

namespace Model.Infrastructure.Services
{
	public interface IPersistentProgressService : IService
	{
		GameProgress GameProgress { get; set; }
	}
}