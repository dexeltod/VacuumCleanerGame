using UnityEngine;

namespace Sources.DomainInterfaces
{
	public interface IMinedResource
	{
		Color Color { get; }
		Material Material { get; }
		GameObject Prefab { get; }
		int Score { get; }
	}
}