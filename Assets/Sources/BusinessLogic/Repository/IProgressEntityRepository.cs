using System.Collections.Generic;
using Sources.DomainInterfaces.Models.Shop.Upgrades;
using Sources.InfrastructureInterfaces.Configs;

namespace Sources.BusinessLogic.Repository
{
	public interface IProgressEntityRepository
	{
		IUpgradeEntityConfig GetConfig(int id);
		IStatUpgradeEntityReadOnly GetEntity(int id);
		IReadOnlyList<IStatUpgradeEntityReadOnly> GetEntities();
		IReadOnlyList<IUpgradeEntityConfig> GetConfigs();
		int GetPrice(int id);
		float GetStatByProgress(int id);
		void AddOneLevel(int id);
	}
}