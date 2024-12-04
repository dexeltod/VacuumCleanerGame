namespace Sources.InfrastructureInterfaces.Factory
{
	public interface IPlayerFactory
	{
		UnityEngine.GameObject Create(
			UnityEngine.GameObject spawnPoint
		);
	}
}