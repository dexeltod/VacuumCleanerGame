using System;
using Sources.DomainInterfaces.Entities;
using Sources.DomainInterfaces.Models;
using Sources.InfrastructureInterfaces.Repository;
using Sources.Utils;
using Sources.Utils.Enums;

namespace Sources.Infrastructure.Repository
{
	public class PlayerModelRepository : IPlayerModelRepository
	{
		private readonly IPlayerModel _playerModel;

		public PlayerModelRepository(IPlayerModel playerModel) =>
			_playerModel = playerModel ?? throw new ArgumentNullException(nameof(playerModel));

		public IStatReadOnly Get(ProgressType type) =>
			_playerModel.Get((int)type);

		public IStatReadOnly Get(int id) =>
			_playerModel.Get(id);

		public void Set(ProgressType type, float value) =>
			_playerModel.Set((int)type, value);
	}
}