using System;
using System.Collections.Generic;
using System.Linq;
using Sources.DomainInterfaces.Models.Shop.Upgrades;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Configs;
using Sources.InfrastructureInterfaces.Repository;
using Sources.ServicesInterfaces;
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

		private IUpgradeProgressRepository UpgradeProgressRepository => _repositoryProvider.Self;

		public float GetProgressStatValue(int id) =>
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