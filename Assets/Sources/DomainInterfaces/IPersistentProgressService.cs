using Sources.DIService;
using Sources.DomainInterfaces;

namespace Sources.ServicesInterfaces
{
	public interface IPersistentProgressService : IService
	{
		IGameProgressModel GameProgress { get; }
		void Construct(IGameProgressModel gameProgressModel);
	}
}