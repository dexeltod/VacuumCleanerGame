using System.Collections.Generic;
using Sources.Domain.Temp;
using Sources.InfrastructureInterfaces.Configs;

namespace Sources.ServicesInterfaces
{
	public interface IProgressService
	{
		float GetProgressStatValue(int id);
		void AddProgressPoint(int id);
		IUpgradeEntityReadOnly GetEntity(int id);
		IReadOnlyList<IUpgradeEntityReadOnly> GetEntities();
		IUpgradeEntityViewConfig GetConfig(int id);
		IReadOnlyList<IUpgradeEntityViewConfig> GetConfigs();
		int GetPrice(int id);
	}
}