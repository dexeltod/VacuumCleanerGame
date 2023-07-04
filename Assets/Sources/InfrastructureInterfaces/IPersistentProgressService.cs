using Domain.Progress;

namespace InfrastructureInterfaces
{
	public interface IPersistentProgressService : IService
	{
		GameProgressModel GameProgress { get; }
		void Construct(GameProgressModel gameProgressModel);
	}
}