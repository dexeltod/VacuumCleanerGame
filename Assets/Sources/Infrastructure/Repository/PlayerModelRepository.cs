using System;
using Sources.BusinessLogic.Repository;
using Sources.DomainInterfaces.Entities;
using Sources.DomainInterfaces.Models;
using Sources.Utils.Enums;

namespace Sources.Infrastructure.Repository
{
	public class PlayerModelRepository : IPlayerModelRepository
	{
		private readonly IPlayerStatsModel _playerStatsModel;

		public PlayerModelRepository(IPlayerStatsModel playerStatsModel) =>
			_playerStatsModel = playerStatsModel ?? throw new ArgumentNullException(nameof(playerStatsModel));

		public IStatReadOnly Get(ProgressType type) => _playerStatsModel.Get((int)type);

		public IStatReadOnly Get(int id) => _playerStatsModel.Get(id);

		public void Set(ProgressType type, float value) => _playerStatsModel.Set((int)type, value);
	}
}