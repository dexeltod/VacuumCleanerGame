using System.Collections.Generic;
using Sources.Domain.Temp;
using Sources.InfrastructureInterfaces.Configs;

namespace Sources.Infrastructure.Repositories
{
	public interface IUpgradeProgressRepository
	{
		IUpgradeEntityViewConfig GetConfig(int id);
		IUpgradeEntityReadOnly GetEntity(int id);
		IReadOnlyList<IUpgradeEntityReadOnly> GetEntities();
		IReadOnlyList<IUpgradeEntityViewConfig> GetConfigs();
		int GetPrice(int id);
		float GetStatByProgress(int id);
		void AddOneLevel(int id);
	}
}