using System;
using Sources.BusinessLogic.Services;
using Sources.DomainInterfaces;

namespace Sources.Infrastructure.Services.DomainServices
{
	public class UpdatablePersistentProgressService : IPersistentProgressService, IUpdatablePersistentProgressService
	{
		private IGlobalProgress _globalProgress;
		public IGlobalProgress GlobalProgress => _globalProgress;

		public UpdatablePersistentProgressService()
		{
		}

		public void Update(IGlobalProgress progress) =>
			_globalProgress = progress ?? throw new ArgumentNullException(nameof(progress));
	}
}
