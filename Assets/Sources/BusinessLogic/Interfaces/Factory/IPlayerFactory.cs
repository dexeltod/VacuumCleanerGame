using Sources.ControllersInterfaces;
using UnityEngine;

namespace Sources.BusinessLogic.Interfaces.Factory
{
	public interface IPlayerFactory
	{
		IMonoPresenter Create(
			GameObject spawnPoint
		);
	}
}