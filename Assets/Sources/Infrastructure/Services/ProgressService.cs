using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Domain.Temp;
using Sources.Infrastructure.Configs;
using Sources.Infrastructure.Repositories;
using Sources.InfrastructureInterfaces.Configs;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Upgrade;
using Sources.Utils;
using VContainer;

namespace Sources.Infrastructure.Services
{
	public class ProgressService : IProgressService
	{
		private readonly UpgradeProgressRepositoryProvider _repositoryProvider;

		[Inject]
		public ProgressService(UpgradeProgressRepositoryProvider repositoryProvider) =>
			_repositoryProvider = repositoryProvider ??
				throw new ArgumentNullException(nameof(repositoryProvider));

		private IUpgradeProgressRepository UpgradeProgressRepository => _repositoryProvider.Implementation;

		public int GetProgressValue(int id) =>
			UpgradeProgressRepository
				.GetConfig(id)
				.Stats
				.ElementAt(
					UpgradeProgressRepository
						.GetEntity(id)
						.Value
				);

		public void AddProgressPoint(int id) =>
			UpgradeProgressRepository.AddOneLevel(id);

		public IUpgradeEntityReadOnly GetEntity(int id) =>
			UpgradeProgressRepository.GetEntity(id);

		public IReadOnlyList<IUpgradeEntityReadOnly> GetEntities() =>
			UpgradeProgressRepository.GetEntities();

		public IUpgradeEntityViewConfig GetConfig(int id) =>
			UpgradeProgressRepository.GetConfig(id);

		public IReadOnlyList<IUpgradeEntityViewConfig> GetConfigs() =>
			UpgradeProgressRepository.GetConfigs();

		public int GetPrice(int id) =>
			UpgradeProgressRepository.GetPrice(id);
	}
}