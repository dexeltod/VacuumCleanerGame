using System.Collections.Generic;
using Sources.Domain.Temp;
using Sources.InfrastructureInterfaces.Configs;

namespace Sources.Infrastructure.Repositories
{
	public interface IUpgradeProgressRepository
	{
		IUpgradeEntityViewConfig GetConfig(int id);
		IProgressEntity GetEntity(int id);
		IReadOnlyList<IProgressEntity> GetEntities();
		IReadOnlyList<IUpgradeEntityViewConfig> GetConfigs();
		int GetPrice(int id);
		int GetStatByProgress(int id);
		void AddOneLevel(int id);
	}
}