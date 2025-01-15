using System.Collections.Generic;
using Sources.DomainInterfaces.Models.Shop.Upgrades;
using Sources.InfrastructureInterfaces.Configs;

namespace Sources.BuisenessLogic.Repository
{
	public interface IProgressEntityRepository
	{
		IUpgradeEntityViewConfig GetConfig(int id);
		IStatUpgradeEntityReadOnly GetEntity(int id);
		IReadOnlyList<IStatUpgradeEntityReadOnly> GetEntities();
		IReadOnlyList<IUpgradeEntityViewConfig> GetConfigs();
		int GetPrice(int id);
		float GetStatByProgress(int id);
		void AddOneLevel(int id);
	}
}