using UnityEngine;

namespace Sources.BuisenessLogic.Interfaces.Factory
{
	public interface IPlayerFactory
	{
		GameObject Create(
			GameObject spawnPoint
		);
	}
}