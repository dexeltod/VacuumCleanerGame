using System.Collections.Generic;
using Sources.DomainInterfaces.Models.Shop.Upgrades;
using Sources.InfrastructureInterfaces.Configs;

namespace Sources.BusinessLogic.Repository
{
	public interface IProgressEntityRepository
	{
		void AddOneLevel(int id);
		IUpgradeEntityConfig GetConfig(int id);
		IReadOnlyList<IUpgradeEntityConfig> GetConfigs();
		IReadOnlyList<IStatUpgradeEntityReadOnly> GetEntities();
		IStatUpgradeEntityReadOnly GetEntity(int id);
		int GetPrice(int id);
		float GetStatByProgress(int id);
	}
}