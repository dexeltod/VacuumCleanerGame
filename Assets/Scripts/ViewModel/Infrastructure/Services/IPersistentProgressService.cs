using Model.Data;

namespace ViewModel.Infrastructure.Services
{
	public interface IPersistentProgressService : IService
	{
		GameProgressModel GameProgress { get; }
		void Construct(GameProgressModel gameProgressModel);
	}
}