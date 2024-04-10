using UnityEngine;

namespace Sources.Infrastructure.Configs.Scripts.Level.LevelResouce
{
	public interface IMinedResource
	{
		Color Color { get; }
		Material Material { get; }
		GameObject Prefab { get; }
		int Score { get; }
	}
}