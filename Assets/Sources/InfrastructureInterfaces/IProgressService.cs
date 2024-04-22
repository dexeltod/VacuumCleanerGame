using System.Collections.Generic;
using Sources.Domain.Temp;
using Sources.InfrastructureInterfaces.Configs;

namespace Sources.ServicesInterfaces
{
	public interface IProgressService
	{
		int GetProgressValue(int id);
		void AddProgressPoint(int id);
		IProgressEntity GetEntity(int id);
		IReadOnlyList<IProgressEntity> GetEntities();
		IUpgradeEntityViewConfig GetConfig(int id);
		IReadOnlyList<IUpgradeEntityViewConfig> GetConfigs();
		int GetPrice(int id);
	}
}