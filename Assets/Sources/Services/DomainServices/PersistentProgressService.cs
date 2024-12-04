using System;
using Sources.DomainInterfaces;

namespace Sources.Services.DomainServices
{
	[Serializable] public class PersistentProgressService : IPersistentProgressService
	{
		private IGlobalProgress _globalProgress;
		public IGlobalProgress GlobalProgress => _globalProgress;

		public PersistentProgressService(IGlobalProgress globalProgress) =>
			_globalProgress = globalProgress ?? throw new ArgumentNullException(nameof(globalProgress));
	}
}