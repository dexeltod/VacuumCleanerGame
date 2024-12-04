using UnityEngine;

namespace Sources.InfrastructureInterfaces
{
	public interface IMinedResource
	{
		Color Color { get; }
		Material Material { get; }
		UnityEngine.GameObject Prefab { get; }
		int Score { get; }
	}
}