using System;
using System.Collections.Generic;
using Sources.Core;

namespace Sources.Infrastructure.Services
{
	public interface IPlayerStatsService : IService
	{
		public int Speed { get; }
		public int VacuumDistance { get; }

		void Set(string name, int value);
	}
}