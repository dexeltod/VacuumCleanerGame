using System.Collections.Generic;

namespace Sources.DomainInterfaces
{
	public interface IGameProgress
	{
		List<IUpgradeProgressData> GetAll();
		IUpgradeProgressData GetByName(string name);
		void Set(string progressName, int progressValue);
	}
}