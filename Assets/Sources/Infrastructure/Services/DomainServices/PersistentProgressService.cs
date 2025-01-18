using System;
using Sources.BusinessLogic.Services;
using Sources.DomainInterfaces;

namespace Sources.Infrastructure.Services.DomainServices
{
	public class PersistentProgressService : IPersistentProgressService, IPersistentProgressServiceUpdatable
	{
		private IGlobalProgress _globalProgress;
		public IGlobalProgress GlobalProgress => _globalProgress;

		public void Update(IGlobalProgress progress) =>
			_globalProgress = progress ?? throw new ArgumentNullException(nameof(progress));
	}
}
