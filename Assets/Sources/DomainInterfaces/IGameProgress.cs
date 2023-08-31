using System.Collections.Generic;

namespace Sources.DomainInterfaces
{
	public interface IGameProgress
	{
		int MaxPointCount { get; }
		List<IUpgradeProgressData> GetAll();
		IUpgradeProgressData GetByName(string name);
		void SetProgress(string progressName, int progressValue);
	}
}