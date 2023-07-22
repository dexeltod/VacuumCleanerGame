using Sources.Core;
using Sources.Core.Domain.Progress;

namespace Sources.DomainServices.Interfaces
{
	public interface IPersistentProgressService : IService
	{
		GameProgressModel GameProgress { get; }
		void Construct(GameProgressModel gameProgressModel);
	}
}