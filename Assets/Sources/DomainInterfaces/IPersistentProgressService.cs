using Sources.DIService;

namespace Sources.DomainInterfaces
{
	public interface IPersistentProgressService : IService
	{
		IGameProgressModel GameProgress { get; }
		void Construct(IGameProgressModel gameProgressModel);
	}
}