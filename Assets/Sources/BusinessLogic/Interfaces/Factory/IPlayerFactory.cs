using UnityEngine;

namespace Sources.BusinessLogic.Interfaces.Factory
{
	public interface IPlayerFactory
	{
		GameObject Create(
			GameObject spawnPoint
		);
	}
}
