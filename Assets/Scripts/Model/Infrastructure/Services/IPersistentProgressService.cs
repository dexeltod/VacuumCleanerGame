using Model.Infrastructure.Data;

namespace Model.Infrastructure.Services
{
	public interface IPersistentProgressService : IService
	{
		GameProgressModel GameProgress { get; set; }
	}
}