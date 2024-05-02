using System.Collections.Generic;
using Sources.DomainInterfaces.Models.Shop.Upgrades;
using Sources.InfrastructureInterfaces.Configs;

namespace Sources.InfrastructureInterfaces
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