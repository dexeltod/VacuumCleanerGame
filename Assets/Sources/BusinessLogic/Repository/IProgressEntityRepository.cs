using System.Collections.Generic;
using Sources.BusinessLogic.Configs;
using Sources.DomainInterfaces.Models.Shop.Upgrades;

namespace Sources.BusinessLogic.Repository
{
	public interface IProgressEntityRepository
	{
		void AddOneLevel(int id);
		IUpgradeEntityViewConfig GetConfig(int id);
		IReadOnlyList<IUpgradeEntityViewConfig> GetConfigs();
		IReadOnlyList<IStatUpgradeEntityReadOnly> GetEntities();
		IStatUpgradeEntityReadOnly GetEntity(int id);
		int GetPrice(int id);
		float GetStatByProgress(int id);
	}
}
