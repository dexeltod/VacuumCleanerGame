using System;
using Sources.BusinessLogic.Services;
using Sources.DomainInterfaces;

namespace Sources.Infrastructure.Services.DomainServices
{
	public class PersistentProgressService : IUpdatablePersistentProgressService
	{
		public PersistentProgressService(IGlobalProgress progress) =>
			GlobalProgress = progress ?? throw new ArgumentNullException(nameof(progress));

		public IGlobalProgress GlobalProgress { get; private set; }

		public void Update(IGlobalProgress progress) =>
			GlobalProgress = progress ?? throw new ArgumentNullException(nameof(progress));
	}
}
