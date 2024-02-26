namespace Sources.InfrastructureInterfaces.Factory
{
	public interface IPlayerFactory
	{
		UnityEngine.GameObject Player { get; }

		UnityEngine.GameObject Create(
			UnityEngine.GameObject spawnPoint
		);
	}
}